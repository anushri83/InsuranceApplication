using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces;

public interface IPolicyService
{
    Task<IEnumerable<Policy>> GetAllPolicyAsync();
    Task<Policy> GetPolicyByPolicyIdAsync(int PolicyId);
    Task AddPolicyAsync(Policy policy);
    Task UpdatePolicyAsync(Policy policy);
    Task DeletePolicyAsync(int id);
}