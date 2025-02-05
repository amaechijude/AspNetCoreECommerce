using System.Text;
using AspNetCoreEcommerce;
using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.ErrorHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

// Load dotnetenv for database connections
DotNetEnv.Env.Load();
var dbHost = Environment.GetEnvironmentVariable("DATABASE_HOST");
var dbUser = Environment.GetEnvironmentVariable("DATABASE_USER");
var dbName = Environment.GetEnvironmentVariable("DATABASE_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
var dbPort = Environment.GetEnvironmentVariable("DATABASE_PORT");

var dbConnectionString = $"Host={dbHost};USer={dbUser};Database={dbName};Password={dbPassword};Port{dbPort}";
builder.Services.AddDbContext<ApplicationDbContext>( options =>
        options.UseNpgsql(dbConnectionString)
    );

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

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// add middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
