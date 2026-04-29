
using Insurance.Domain.Models;

namespace Insurance.Domain.Interfaces;

public interface IPolicyRepository
{
    Task<IEnumerable<Policy>> GetAllPoliciesAsync();
    Task<Policy> GetPolicyByIdAsync(int id);
    Task AddPolicyAsync(Policy policy);
}
