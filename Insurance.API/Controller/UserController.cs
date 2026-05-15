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

    [HttpGet("Id")]
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
    [HttpGet("email")]
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
    

    [HttpPost]
    public async Task<IActionResult> AddUserAsync(User user)
    {
        try
        {
            if (user == null)
            {
                return BadRequest("Data is null");
            }
            await _userService.AddUserAsync(user);
            return Ok($"User Added successfully at {DateTime.Now}");
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

    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync(User user)
    {
        try
        {
            if (user == null)
            {
                return BadRequest("Data is null");
            }
            await _userService.UpdateUserAsync(user);
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
    [HttpDelete]
    public async Task<IActionResult> DeleteUserAsync(User user)
    {
        try
        {
            await _userService.DeleteUserAsync(user);
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

