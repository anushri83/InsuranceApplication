using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetUsersByIdAsync(int Id)
    {
        try
        {
            User user = await _userService.GetUserByIdAsync(Id);
            if (user == null)
            {
                return NotFound($"user with {Id} not found");
            }
            return Ok(user);
        }
        catch (Exception)
        {
            return BadRequest("An error occurred while fetching the user.");
        }
    }
    [HttpGet("email")]
    public async Task<IActionResult> GetUserByEmailAsync(string email)
    {
        try
        {
            User user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"user with {email} not found");
            }
            return Ok(user);
        }
        catch (Exception)
        {
            return BadRequest("An error occurred while fetching the user.");
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
    }
}

