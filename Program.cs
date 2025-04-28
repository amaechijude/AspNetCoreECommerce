using System.Text;
using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.UseCases.Authentication;
using AspNetCoreEcommerce.Application.UseCases.CartUseCase;
using AspNetCoreEcommerce.Application.UseCases.CustomerUseCase;
using AspNetCoreEcommerce.Application.UseCases.OrderUseCase;
using AspNetCoreEcommerce.Application.UseCases.PaymentUseCase;
using AspNetCoreEcommerce.Application.UseCases.ProductUseCase;
using AspNetCoreEcommerce.Application.UseCases.ShippingAddressUseCase;
using AspNetCoreEcommerce.Application.UseCases.VendorUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.Data;
using AspNetCoreEcommerce.Infrastructure.PaymentChannel;
using AspNetCoreEcommerce.Infrastructure.Repositories;
using AspNetCoreEcommerce.Shared;
using AspNetCoreEcommerce.Shared.EmailConstants;
using AspNetCoreEcommerce.Shared.ErrorHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// verfy email settings
EmailSettings.Configure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
// Load dotnetenv for database connections
// Load dotnev
DotNetEnv.Env.Load();

//Db Config
var dbHost = Environment.GetEnvironmentVariable("DATABASE_HOST");
var dbUser = Environment.GetEnvironmentVariable("DATABASE_USER");
var dbName = Environment.GetEnvironmentVariable("DATABASE_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
var dbPort = Environment.GetEnvironmentVariable("DATABASE_PORT");

// Construct connection string
// var environment = builder.Environment;
var dbConnectionString = $"Host={dbHost};Username={dbUser};Database={dbName};Password={dbPassword};Port={dbPort}";

//Register Application context with postgresql connection
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(dbConnectionString));

//Authentication and Identity

builder.Services.AddAuthorization();

builder.Services.AddIdentity<User, UserRole>(options => {
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Register JWT
builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        var secretKey = $"{Environment.GetEnvironmentVariable("JWT_SECRET_KEY")}";
        options.RequireHttpsMetadata = false; // Set to true in production
        options.SaveToken = true; // Save the token in the authentication properties
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidIssuer = $"{Environment.GetEnvironmentVariable("JWT_ISSUER")}",
            ValidAudiences = [GlobalConstants.customerRole, GlobalConstants.vendorRole],
            ClockSkew = TimeSpan.Zero,
        };
    });


// Register Repositories
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IShippingAddressRespository, ShippingAddressRespository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();

// Register Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IShippingAddressService, ShippingAddressService>();
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderSevice, OrderSevice>();

builder.Services.AddScoped<ErcasPay>(); // Payment channel service


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Image folder storage
var imageFilePath = Path.Combine(builder.Environment.ContentRootPath, GlobalConstants.uploadPath);
if (!Directory.Exists(imageFilePath))
    Directory.CreateDirectory(imageFilePath);

app.UseStaticFiles(new StaticFileOptions{
    FileProvider = new PhysicalFileProvider(imageFilePath),
    RequestPath = $"/{GlobalConstants.uploadPath}"
});

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowFrontend");
// add middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();