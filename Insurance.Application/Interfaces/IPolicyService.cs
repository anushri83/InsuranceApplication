using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces;

public interface IPolicyService
{
    Task<IEnumerable<Policy>> GetPoliciesAsync();
    Task<Policy> GetPolicyByIdAsync(int id);
    Task AddPolicyAsync(Policy policy);
}