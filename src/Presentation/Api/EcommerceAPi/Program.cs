using Data;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Load dotnev
DotNetEnv.Env.Load();
var dbHost = Environment.GetEnvironmentVariable("DATABASE_HOST");
var dbUser = Environment.GetEnvironmentVariable("DATABASE_USER");
var dbName = Environment.GetEnvironmentVariable("DATABASE_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
var dbPort = Environment.GetEnvironmentVariable("DATABASE_PORT");

// Construct connection string
var dbConnectionString = $"Host={dbHost};Username={dbUser};Database={dbName};Password={dbPassword};Port={dbPort}";
//Register Application context with postgresql connection
builder.Services.AddDbContext<ApplicationDbContext>(services => services.UseNpgsql(dbConnectionString));

// Register Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();

// Register Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IVendorService, VendorService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
