using Insurance.Domain.Models;

namespace Insurance.Domain.Interfaces;

public interface IPolicyRepository
{
    Task<IEnumerable<Policy>> GetAllPolicyAsync();
    Task<Policy> GetPolicyByPolicyIdAsync(int PolicyId);
    Task AddPolicyAsync(Policy policy);
    Task UpdatePolicyAsync(Policy policy);
    Task DeletePolicyAsync(int PolicyId);
}
