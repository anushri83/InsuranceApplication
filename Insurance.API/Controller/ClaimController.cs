using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimService _claimService;

        public ClaimsController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClaimsAsync()
        {
            var claims = await _claimService.GetAllClaimsAsync();
            return Ok(claims);
        }


        [HttpGet("claim/{ClaimId}")]
        public async Task<IActionResult> GetClaimByClaimIdAsync(int ClaimId)
        {
            try
            {
                var claim = await _claimService.GetClaimByClaimIdAsync(ClaimId);
                return Ok(claim);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("user/{UserId}")]
        public async Task<IActionResult> GetClaimsByUserIdAsync(int UserId)
        {
            try
            {
                var claims = await _claimService.GetClaimsByUserIdAsync(UserId);
                return Ok(claims);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddClaimAsync([FromBody] Claim claim) 
        {
            try
            {
                if (!ModelState.IsValid)    // Checks if the incoming data is valid based on the model's data annotations
                {
                    return BadRequest(ModelState);
                }
                await _claimService.AddClaimAsync(claim);
                return Ok("Claim submitted successfully and is now Pending.");
                //return CreatedAtAction(nameof(GetClaimByClaimIdAsync), new { id = newClaim.ClaimId }, newClaim);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        //(Admin action)
        [HttpPut("{ClaimId}/approve")]
        public async Task<IActionResult> Approve(int ClaimId)
        {
            try
            {
                await _claimService.ApproveClaimAsync(ClaimId);
                return Ok("Claim has been Approved.");
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


        // (Admin action)
        [HttpPut("{ClaimId}/reject")]
        public async Task<IActionResult> RejectClaimAsync(int ClaimId, [FromBody] string reason)
        {
            try
            {
                await _claimService.RejectClaimAsync(ClaimId, reason);
                return Ok("Claim has been Rejected.");
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

        
        [HttpDelete("{ClaimId}")]
        public async Task<IActionResult> DeleteClaimAsync(int ClaimId)
        {
            try
            {
                await _claimService.DeleteClaimAsync(ClaimId);
                return Ok("Claim record deleted.");
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
}