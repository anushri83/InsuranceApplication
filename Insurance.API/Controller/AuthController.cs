using Insurance.Application.DTOs.ClaimDTO;
using Insurance.Application.DTOs.UserDTO;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController :ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
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
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during authentication: {ex.Message}");
            }
        }
    }
}
