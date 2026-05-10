using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers;

[ApiController] // Tells .NET this class handles API requests
[Route("api/[controller]")] // Sets the URL to: api/policy

public class PolicyController : ControllerBase
{
    private readonly IPolicyService _policyService;
    public PolicyController(IPolicyService policyService)
    {
        _policyService = policyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPoliciesAsync()
    {
        try
        {
            var policies = await _policyService.GetAllPolicyAsync();
            return Ok(policies); // Returns HTTP 200 with the data
        }
        catch (Exception ex)
        {
            // If the database is down, this will catch the error
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPolicyByPolicyIdAsync(int PolicyId)
    {
        try
        {
            var policy = await _policyService.GetPolicyByPolicyIdAsync(PolicyId);
            return Ok(policy);
        }
        catch (Exception ex)
        {
            return  NotFound(ex);
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> AddPolicyAsync(Policy policy)
    {
        try
        {
            await _policyService.AddPolicyAsync(policy);
            return Ok("Policy Added successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Invalid Data: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }

    }

    [HttpPut]
    public async Task<IActionResult> UpdatePolicyAsync(Policy policy)
    {
        try
        {
            await _policyService.UpdatePolicyAsync(policy);
            return Ok("Policy Updated successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Invalid Data: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }

    }

    [HttpDelete]
    public async Task<IActionResult> DeletePolicyAsync(int policyId)
    {
        try
        {
            await _policyService.DeletePolicyAsync(policyId);
            return Ok("Policy deleted successfully");
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }

    }

}
