using Insurance.Application.DTOs.UserDTO;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Insurance.API.Controllers;

[ApiController] // Tells .NET this class handles API requests
[Route("api/[controller]")] // Sets the URL to: api/policy
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
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
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto dto)
    {
        try
        {
            await _userService.UpdateUserAsync(dto);
            return Ok($"User Updated successfully at {DateTime.Now}");
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
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto dto)
    {
        try
        {
            await _userService.ChangePasswordAsync(dto);
            return Ok($"Password Changed successfully at {DateTime.Now}");
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

        [HttpDelete]
    public async Task<IActionResult> DeleteUserAsync(int  userId)
    {
        try
        {
            await _userService.DeleteUserAsync(userId);
            return Ok($"User Deleted successfully at {DateTime.Now}");
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

