using Insurance.Domain.Models;

namespace Insurance.Domain.Interfaces
{
    public interface ICustomerPolicyRepository
    {
        Task<IEnumerable<CustomerPolicy>> GetAllCustomerPoliciesAsync();
        Task<CustomerPolicy> GetCustomerPolicyByIdAsync(int customerPolicyId);
        Task<IEnumerable<CustomerPolicy>> GetByUserIdAsync(int userId);
        Task AddCustomerPolicyAsync(CustomerPolicy customerPolicy);
        Task UpdateCustomerPolicyAsync(CustomerPolicy customerPolicy);
        Task DeleteCustomerPolicyAsync(int customerPolicyId);
    }
}
