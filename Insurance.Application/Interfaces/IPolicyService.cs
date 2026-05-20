using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces;

public interface IPolicyService
{

    Task<IEnumerable<Policy>> GetAllPoliciesAsync();
    Task<Policy> GetPolicyByPolicyIdAsync(int policyId);
    Task CreatePolicyAsync(Policy policy);
    Task UpdatePolicyAsync(Policy policy);
    Task DeletePolicyAsync(int id);
    Task<IEnumerable<Policy>> GetActivePoliciesAsync();
    Task<IEnumerable<Policy>> GetInActivePoliciesAsync();
}