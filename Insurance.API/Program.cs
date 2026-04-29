using Insurance.Infrastructure.Data; // Access to your DB Context
using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- STEP A: Add Services to the Container ---

// 1. Add API Documentation (OpenAPI/Swagger)
builder.Services.AddOpenApi();
builder.Services.AddControllers(); // Required if you use Controllers

// 2. Register the DbContext (Connects to Infrastructure)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();

// 3. Register your Application Services (Dependency Injection)
// Example: builder.Services.AddScoped<IPolicyService, PolicyService>();

var app = builder.Build();

// --- STEP B: Configure the HTTP Pipeline ---

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // If you want the Swagger UI page, you usually add app.UseSwaggerUI() here
}

app.UseHttpsRedirection();
app.UseAuthorization();

// This line tells the API to find your Policy/User controllers
app.MapControllers();

app.Run();