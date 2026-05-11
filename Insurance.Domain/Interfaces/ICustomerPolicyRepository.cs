using Insurance.Domain.Models;

namespace Insurance.Domain.Interfaces
{
    public interface ICustomerPolicyRepository
    {
        // Retrieves every customer policy link in the database for administrative reporting
        Task<IEnumerable<CustomerPolicy>> GetAllCustomerPoliciesAsync();

        // Fetches a specific policy purchase record including details of the customer and the product
        Task<CustomerPolicy> GetCustomerPolicyByIdAsync(int customerPolicyId);

        // Retrieves all insurance policies purchased by a specific customer to show on their dashboard
        Task<IEnumerable<CustomerPolicy>> GetByUserIdAsync(int userId);

        // Saves a new policy purchase record when a customer buys a plan
        Task AddCustomerPolicyAsync(CustomerPolicy customerPolicy);

        // Updates details of an existing customer policy, such as renewing the end date or changing status
        Task UpdateCustomerPolicyAsync(CustomerPolicy customerPolicy);

        // Removes a policy link from a customer record, usually in cases of data entry errors
        Task DeleteCustomerPolicyAsync(int customerPolicyId);

        // Retrieves all policies sold by a specific agent to track their sales performance and commissions
        Task<IEnumerable<CustomerPolicy>> GetByAgentIdAsync(int agentId);
    }
}