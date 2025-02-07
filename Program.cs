using System.Text;
using AspNetCoreEcommerce;
using AspNetCoreEcommerce.Authentication;
using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.ErrorHandling;
using AspNetCoreEcommerce.Repositories.Implementations;
using AspNetCoreEcommerce.Respositories.Contracts;
using AspNetCoreEcommerce.Respositories.Implementations;
using AspNetCoreEcommerce.Services.Contracts;
using AspNetCoreEcommerce.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

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
var dbConnectionString = $"Host={dbHost};Username={dbUser};Database={dbName};Password={dbPassword};Port={dbPort}";

//Register Application context with postgresql connection
builder.Services.AddDbContext<ApplicationDbContext>(services =>
    services.UseNpgsql(dbConnectionString)
    );


// Register JWT
builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        var secretKey = $"{Environment.GetEnvironmentVariable("JWT_SECRET_KEY")}";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidIssuer = $"{Environment.GetEnvironmentVariable("JWT_ISSUER")}",
            ValidAudiences = [GlobalConstants.customerRole, GlobalConstants.vendorRole],
            ClockSkew = TimeSpan.Zero,
        };
    });


// Register Repositories
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
// builder.Services.AddScoped<IOrderRepository, OrderRepository>();
// builder.Services.AddScoped<>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();

// Register Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IProductService, ProductService>();


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


// add middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
