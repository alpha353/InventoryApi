using FluentValidation;
using InventoryApi.Data;
using InventoryApi.Middleware;
using InventoryApi.Repositories;
using InventoryApi.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Swagger/OpenAPI Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "InventoryApi", Version = "v1" });
});

// Database Configuration

// For the purpose of this task, I'll use an in-memory database or a default connection string if not provided,
// but the requirement asked for PostgreSQL. Use a placeholder connection string if not set.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? "Host=localhost;Database=InventoryDb;Username=postgres;Password=yourpassword";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<StockAdjustmentValidator>();

var app = builder.Build();

// Standardize Middleware Order
app.UseMiddleware<GlobalExceptionMiddleware>(); // Exception handling first

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "InventoryApi v1");
    c.RoutePrefix = string.Empty; // Serve at root
});

app.MapGet("/debug", () => "Hello from API Root!");

// app.UseHttpsRedirection(); // Removed for Docker demo 

app.UseAuthorization();

app.MapControllers();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    await DbInitializer.SeedAsync(scope.ServiceProvider);
}

app.Run();
