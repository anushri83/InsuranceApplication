using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces
{
    public interface ICustomerPolicyService
    {
        Task<IEnumerable<CustomerPolicy>> GetAllCustomerPoliciesAsync();
        Task<CustomerPolicy> GetCustomerPolicyByIdAsync(int customerPolicyId);
        Task<IEnumerable<CustomerPolicy>> GetByUserIdAsync(int userId);
        Task<IEnumerable<CustomerPolicy>> GetByAgentIdAsync(int agentId);
        Task<IEnumerable<User>> GetCustomersByAgentIdAsync(int agentId);
        Task AddCustomerPolicyAsync(CustomerPolicy customerPolicy);
        Task UpdateCustomerPolicyAsync(CustomerPolicy customerPolicy);
        Task DeleteCustomerPolicyAsync(int customerPolicyId);
        Task<decimal> CalculateAgentCommissionAsync(int agentId);

    }
}
