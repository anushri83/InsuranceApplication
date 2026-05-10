using Insurance.Infrastructure.Data; // Access to your DB Context
using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Insurance.Domain.Interfaces;
using Insurance.Infrastructure.Repositories;
using Insurance.Application.Interfaces;
using Insurance.Application.Services;


// This is the entry point of your ASP.NET Core Web API application
var builder = WebApplication.CreateBuilder(args);


// --- STEP A: Add Services to the Container ---

// 1. Add API Documentation (OpenAPI/Swagger)
builder.Services.AddOpenApi();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler=System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; //avoid infinite loops in JSON serialization when you have circular references in your models
});


// 2. Register the DbContext (Connects to Infrastructure)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


// 3. Register your Application Repositories (Dependency Injection)
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerPolicyRepository, CustomerPolicyRepository>();


// 4. Register your Application Services (Dependency Injection)
builder.Services.AddScoped<IPolicyService, PolicyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomerPolicyService, CustomerPolicyService>();


// --- STEP B: Configure the HTTP Pipeline ---

// This is needed for Swagger to discover your API endpoints
builder.Services.AddEndpointsApiExplorer();

// This adds the Swagger generator, which creates the OpenAPI specification for your API
builder.Services.AddSwaggerGen();

// This is where you configure the middleware that will handle HTTP requests
var app = builder.Build();  


// Only enable Swagger in development mode for security reasons
if (app.Environment.IsDevelopment())  
{
    app.MapOpenApi();  
    app.UseSwagger(); 
    app.UseSwaggerUI();
}

// Redirect HTTP requests to HTTPS for better security
app.UseHttpsRedirection();

// This is where you would add authentication middleware 
app.UseAuthorization();   

// This line tells the API to find your Policy/User controllers
app.MapControllers();

// Finally, run the application
app.Run();