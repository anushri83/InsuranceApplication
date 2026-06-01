using Insurance.Application.DTOs.ClaimDTO;
using Insurance.Application.DTOs.UserDTO;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers
{
    [Authorize] // Ensures only authenticated users can access these endpoints
    [ApiController] // Tells .NET this class handles API requests
    [Route("api/[controller]")] // Sets the URL to: api/policy

    public class AuthController :ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var token = await _authService.LoginAsync(loginDTO);
                if(token == null)
                {
                    return Unauthorized("Invalid credentials");
                }
                return Ok(new { Token = token });
            }
            //catch (Exception ex)
            //{
            //    return StatusCode(500, $"An error occurred during authentication: {ex.Message}");
            //}
            catch (Exception ex)
            {
                // 💡 Temporarily swap this out so we can read the raw stack trace in Swagger/Postman
                return StatusCode(500, $"Debug Error: {ex.ToString()}");
            }
        }
    }
}
