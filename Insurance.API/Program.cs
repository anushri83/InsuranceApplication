using Insurance.Api.BackgroundServices;
using Insurance.Application.Interfaces;
using Insurance.Application.Services;
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data; // Access to your DB Context
using Insurance.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


// This is the entry point of your ASP.NET Core Web API application
var builder = WebApplication.CreateBuilder(args);


// 1. Add API Documentation (OpenAPI/Swagger)
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
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();


// 4. Register your Application Services (Dependency Injection)
builder.Services.AddScoped<IPolicyService, PolicyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomerPolicyService, CustomerPolicyService>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHostedService<PolicyExpiryWorker>();

// --- STEP B: Configure the HTTP Pipeline ---

// This is needed for Swagger to discover your API endpoints
builder.Services.AddEndpointsApiExplorer();


// This adds the Swagger generator, which creates the OpenAPI specification for your API
builder.Services.AddSwaggerGen();

// Add this under your other builder.Services definitions
builder.Services.AddMemoryCache();

//  Fetch JWT configurations from appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"]
    ?? throw new InvalidOperationException("JWT Secret Key is missing.");


//Adds authentication functionality to the application.
builder.Services.AddAuthentication(options =>
{
    // Tells ASP.NET Core to use JWT Bearer Tokens by default.
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})


//Adds JWT token validation middleware.
.AddJwtBearer(options =>
{
    //Defines rules for validating JWT token.
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,  //verifies token creator
        ValidateAudience = true, //verifies intended application/user
        ValidateLifetime = true, //checks token expiration
        ValidateIssuerSigningKey = true, //verifies token signature using SecretKey

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        //Converts secret string into encrypted security key.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});


// This is where you configure the middleware that will handle HTTP requests
var app = builder.Build();

// Only enable Swagger in development mode for security reasons
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP requests to HTTPS for better security
app.UseHttpsRedirection();

//every request hitting your API goes through a security checkpoint before it ever hits your controller files
// MUST BE IN THIS EXACT ORDER
app.UseAuthentication(); // Checks WHO the user is (Reads the token)
app.UseAuthorization();  // Checks WHAT the user can do (Checks their role)

// This line tells the API to find your controllers
app.MapControllers();

// Finally, run the application
app.Run();