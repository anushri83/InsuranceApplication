using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers;

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
    [HttpGet("{customerPolicyId}")]
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


    [HttpPost]
    public async Task<IActionResult> AddCustomerPolicyAsync(CustomerPolicy customerPolicy)
    {
        try
        {
             await _customerPolicyService.AddCustomerPolicyAsync(customerPolicy);
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
    public async Task<IActionResult> UpdateCustomerPolicyAsync(CustomerPolicy customerPolicy)
    {
        try
        {
            await _customerPolicyService.UpdateCustomerPolicyAsync(customerPolicy);
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
    [HttpDelete]
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


