using Insurance.Application.DTOs.UserDTO;
using Insurance.Application.Interfaces;
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Insurance.Application.Services;

namespace Insurance.API.Controllers;

[Authorize] // Ensures only authenticated users can access these endpoints
[ApiController] // Tells .NET this class handles API requests
[Route("api/[controller]")] // Sets the URL to: api/policy
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IMemoryCache _cache;

    public UserController(IUserService userService, IMemoryCache cache)
    {
        _userService = userService;
        _cache = cache;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")] // Only Admins can access this endpoint
    public async Task<IActionResult> GetAllUsersAsync()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("user/{UserId}")]
    [Authorize]
    public async Task<IActionResult> GetUsersByIdAsync(int UserId)
    {
        try
        {
            User user = await _userService.GetUserByIdAsync(UserId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("email/{email}")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> GetUserByEmailAsync(string email)
    {
        try
        {
            User user = await _userService.GetUserByEmailAsync(email);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }


    [HttpGet("role/{role}")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> GetUsersByRoleAsync(UserRole role)
    {
        try
        {
            var users = await _userService.GetUsersByRoleAsync(role);                
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }


    [HttpPost("register/customer")]
    [AllowAnonymous] // Allows unauthenticated users to access this endpoint
    public async Task<IActionResult> RegisterCustomer([FromBody] CreateCustomerDto dto)
    {
        try
        {
            if(!ModelState.IsValid)    // Checks if the incoming data is valid based on the model's data annotations
            {
                return BadRequest(ModelState);
            }

            await _userService.RegisterCustomerAsync(dto);
            return Ok("Customer registered successfully!");
                   }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // Catches our 18+ age or duplicate email errors
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPost("Register/Agent")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> RegisterAgent(CreateAgentDto dto)
    {
        try
        {
            if(!ModelState.IsValid)    // Checks if the incoming data is valid based on the model's data annotations
            {
                return BadRequest(ModelState);
            }
             await _userService.RegisterAgentAsync(dto);
            return Ok("Agent registered successfully!");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // Catches our 18+ age or duplicate email errors
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    [HttpPost("Register/Admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterAdmin(CreateAdminDto dto)
    {
        try
        {
            if (!ModelState.IsValid)    // Checks if the incoming data is valid based on the model's data annotations
            {
                return BadRequest(ModelState);
            }
            await _userService.RegisterAdminAsync(dto);
            return Ok("Admin registered successfully!");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // Catches our 18+ age or duplicate email errors
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPut("update-user")]
    [Authorize]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto dto)
    {
        try
        {
            await _userService.UpdateUserAsync(dto);
            return Ok($"User Updated successfully at {DateTime.UtcNow}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto dto)
    {
        try
        {
            await _userService.ChangePasswordAsync(dto);
            return Ok($"Password Changed successfully at {DateTime.UtcNow}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(string Email)
    {
        var user = await _userService.GetUserByEmailAsync(Email);

        if (user == null)
        {
            return BadRequest("Email address not found.");
        }
        // 1. Generate the 6-digit OTP
        var random = new Random();
        string generatedOtp = random.Next(100000, 999999).ToString();

        // 2. Configure the 1-minute absolute expiration rule
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

        // 3. Save the OTP directly to RAM using the email as a unique locker key
        string cacheKey = $"OTP_{Email}";
        _cache.Set(cacheKey, generatedOtp, cacheOptions);

        Console.WriteLine($"====================================");
        Console.WriteLine($"[RAM CACHE OTP] -> {Email}: {generatedOtp} (Expires in 60s)");
        Console.WriteLine($"====================================");
        // 3. If it is NOT null, your server generates the link/OTP here in the background
        // _emailService.SendResetLink(user.Email, generatedToken); 
        return Ok("A password reset link has been sent to your email.");
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var user = await _userService.GetUserByEmailAsync(dto.Email);
        if (user == null)
        {
            return BadRequest("Invalid request.");
        }
        // 1. Attempt to fetch the OTP from RAM using the email key
        string cacheKey = $"OTP_{dto.Email}";
        if (!_cache.TryGetValue(cacheKey, out string storedOtp))
        {
            // If the key doesn't exist, it means the 1-minute timer ran out and RAM deleted it!
            return BadRequest("The OTP has expired or is invalid. Please request a new one.");
        }

        // 2. Verify if the entered OTP matches the cached OTP
        if (storedOtp != dto.OTP)
        {
            return BadRequest("Invalid OTP token.");
        }
        // 2. Secretly verify if the Token/OTP is valid (Hypothetical verification layer)
        // bool isTokenValid = await _userService.VerifyTokenAsync(user.UserId, dto.Token);
        // if (!isTokenValid) return BadRequest("Invalid or expired token.");

        user.PasswordHash = _userService.HashPassword(dto.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;


        await _userRepository.UpdateUserAsync(user); // Push changes down to repository

        return Ok("Your password has been reset successfully. You can now log in!");
    }


    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUserAsync(int  userId)
    {
        try
        {
            await _userService.DeleteUserAsync(userId);
            return Ok($"User Deleted successfully at {DateTime.UtcNow}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}

