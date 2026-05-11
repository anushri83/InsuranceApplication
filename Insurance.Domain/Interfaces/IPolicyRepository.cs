using Insurance.Domain.Models;

namespace Insurance.Domain.Interfaces
{
    public interface IPolicyRepository
    {
        // Retrieves the complete list of all insurance products available in the database
        Task<IEnumerable<Policy>> GetAllPolicyAsync();

        // Fetches the specific details of a single insurance plan using its unique identifier
        Task<Policy?> GetPolicyByPolicyIdAsync(int PolicyId);

        // Inserts a new insurance product into the system's catalog
        Task AddPolicyAsync(Policy policy);

        // Modifies the terms, premiums, or details of an existing insurance policy
        Task UpdatePolicyAsync(Policy policy);

        // Removes an insurance product from the catalog permanently
        Task DeletePolicyAsync(int PolicyId);

        // Retrieves only the insurance plans that are currently flagged as active for customers to purchase in the shop
        Task<IEnumerable<Policy>> GetActivePoliciesAsync();
    }
}