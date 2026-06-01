using Insurance.Application.DTOs.CustomerPolicyDTO;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers;

[Authorize] // Ensures only authenticated users can access these endpoints
[ApiController] // Tells .NET this class handles API requests
[Route("api/[controller]")] // Sets the URL to: api/policy

public class CustomerPolicyController : ControllerBase
{
    private readonly ICustomerPolicyService _customerPolicyService;

    public CustomerPolicyController(ICustomerPolicyService customerPolicyService)
    {
        _customerPolicyService = customerPolicyService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllCustomerPoliciesAsync()
    {
        try
        {
            var customerPolicies = await _customerPolicyService.GetAllCustomerPoliciesAsync();
            return Ok(customerPolicies);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
        
    }

    [HttpGet("customePolicy/{customerPolicyId}")]
    [Authorize]
    public async Task<IActionResult> GetCustomerPolicyByIdAsync(int customerPolicyId)
    {
        try
        {
            var customerPolicies = await _customerPolicyService.GetCustomerPolicyByIdAsync(customerPolicyId);
            return Ok(customerPolicies);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("user/{userId}")]
    [Authorize]
    public async Task<IActionResult> GetByUserIdAsync(int userId)
    {
        try
        {
            var customerPolicies = await _customerPolicyService.GetByUserIdAsync(userId);
            return Ok(customerPolicies);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("agent/{agentId}")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> GetByAgentIdAsync(int agentId)
    {
        try
        {
            var customerPolicies = await _customerPolicyService.GetByAgentIdAsync(agentId);
            return Ok(customerPolicies);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("agent/{agentId}/customers")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> GetCustomersByAgentIdAsync(int agentId)
    {
        try
        {
           var customers = await _customerPolicyService.GetCustomersByAgentIdAsync(agentId);
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("{agentId}/Commission")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> CalculateAgentCommissionAsync(int agentId)
    {
        try
        {
            decimal commission = await _customerPolicyService.CalculateAgentCommissionAsync(agentId);
            return Ok(new
            {
                AgentId = agentId,
                TotalCommissionEarned = commission
            });
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred calculating commission: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddCustomerPolicyAsync([FromBody] PurchasePolicyDto dto)
    {
        try
        {
            if (!ModelState.IsValid)    // Checks if the incoming data is valid based on the model's data annotations
            {
                return BadRequest(ModelState);
            }
            await _customerPolicyService.AddCustomerPolicyAsync(dto);
            return Ok("Customer policy added successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Invalid premium amount: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> UpdateCustomerPolicyAsync([FromBody] UpdateCustomerPolicyStatusDto dto)
    {
        try
        {
            await _customerPolicyService.UpdateCustomerPolicyAsync(dto);
            return Ok("Customer policy updated successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Invalid premium amount: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpDelete("{customerPolicyId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCustomerPolicyAsync(int customerPolicyId)
    {
        try
        {
           await _customerPolicyService.DeleteCustomerPolicyAsync(customerPolicyId);
            return Ok("Customer policy deleted successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Invalid premium amount: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    

}


