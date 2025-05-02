using System.Text;
using System.Threading.Channels;
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
using AspNetCoreEcommerce.Infrastructure.Data.Seeders;
using AspNetCoreEcommerce.Infrastructure.EmailInfrastructure;
using AspNetCoreEcommerce.Infrastructure.PaymentChannel;
using AspNetCoreEcommerce.Infrastructure.Repositories;
using AspNetCoreEcommerce.Shared;
using AspNetCoreEcommerce.Shared.ErrorHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;

// Logging
string logPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
if (!Directory.Exists(logPath))
    Directory.CreateDirectory(logPath);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File($"{logPath}/log-.txt", rollingInterval: RollingInterval.Hour)
    .CreateLogger();

try
{
    Log.Information("Starting web host...");
    Log.Information($"Current Directory: {Directory.GetCurrentDirectory()}");

    var builder = WebApplication.CreateBuilder(args);
    // Add services to the container.

    // Load dotnetenv for database connections
    // Load dotnev
    DotNetEnv.Env.Load();
    // Load environment variables from .env file
    //Db Config
    string dbHost = Environment.GetEnvironmentVariable("DATABASE_HOST")
        ?? throw new ArgumentException("DATABASE_HOST", "Database host is not set in the environment variables.");
    string dbUser = Environment.GetEnvironmentVariable("DATABASE_USER")
        ?? throw new ArgumentException("DATABASE_USER", "Database user is not set in the environment variables.");
    string dbName = Environment.GetEnvironmentVariable("DATABASE_NAME")
        ?? throw new ArgumentException("DATABASE_NAME", "Database name is not set in the environment variables.");
    string dbPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD")
        ?? throw new ArgumentException("DATABASE_PASSWORD", "Database password is not set in the environment variables.");
    string dbPort = Environment.GetEnvironmentVariable("DATABASE_PORT")
        ?? throw new ArgumentException("DATABASE_PORT", "Database port is not set in the environment variables.");

    // Construct connection string
    // var environment = builder.Environment;
    string dbConnectionString = $"Host={dbHost};Port={dbPort};Username={dbUser};Password={dbPassword};Database={dbName}";

    //Register Application context with postgresql connection
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(dbConnectionString));

    // Register Identity services with PostgreSQL
    builder.Services.AddIdentity<User, UserRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
    })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    // Register the token provider
    builder.Services.AddSingleton<TokenProvider>();

    // Register JWT
    string jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
        ?? throw new ArgumentException("JWT_SECRET_KEY", "Jwt secret key is not set in the environment variables.");
    string jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
        ?? throw new ArgumentException("JWT_ISSUER", "Jwt issuer is not set in the environment variables.");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.RequireHttpsMetadata = builder.Environment.IsProduction();
            options.SaveToken = true; // Save the token in the authentication properties

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
            };
        });

    // --- Authorization Configuration ---
    builder.Services.AddAuthorization();

    // --- CORS Configuration ---
    const string CorsPolicyName = "AllowFrontEnd";
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(CorsPolicyName, policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    // --- Dependency Injection (Repositories & Services) ---
    // Repositories
    builder.Services.AddScoped<ICartRepository, CartRepository>();
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    builder.Services.AddScoped<IShippingAddressRespository, ShippingAddressRespository>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
    builder.Services.AddScoped<IVendorRepository, VendorRepository>();

    // Services
    builder.Services.AddScoped<ICustomerService, CustomerService>();
    builder.Services.AddScoped<IShippingAddressService, ShippingAddressService>();
    builder.Services.AddScoped<IVendorService, VendorService>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IPaymentService, PaymentService>();
    builder.Services.AddScoped<ICartService, CartService>();
    builder.Services.AddScoped<IOrderSevice, OrderSevice>();

    // Payment channel service
    builder.Services.AddScoped<ErcasPay>();

    // Payment channel service
    builder.Services.AddSingleton<EmailService>(); // Email service
    builder.Services.AddSingleton(Channel.CreateBounded<EmailDto>(100));
    builder.Services.AddHostedService<EmailService.EmailBackgroundService>();

    // --- API Controllers & OpenAPI ---
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    // --- Build the Application ---
    var app = builder.Build();

    // --- Configure the HTTP request pipeline ---

    // Custom ExceptionHandling Middleware
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Seed role
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            // Seed the database with roles
            await SeedDatabase.SeedRoleAsync(services);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while seeding the database.");
        }
    }

    // Scalar Config
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    // Image folder storage setup
    var imageFilePath = Path.Combine(builder.Environment.ContentRootPath, GlobalConstants.uploadPath);
    if (!Directory.Exists(imageFilePath))
        Directory.CreateDirectory(imageFilePath);

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(imageFilePath),
        RequestPath = $"/{GlobalConstants.uploadPath}"
    });

    app.UseRouting(); // Determines endpoint
    app.UseCors(CorsPolicyName); // Cors policy name

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed.");
}
finally
{
    Log.CloseAndFlush();
}