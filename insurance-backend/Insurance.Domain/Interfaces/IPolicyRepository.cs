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
        Task CreatePolicyAsync(Policy policy);

        // Modifies the terms, premiums, or details of an existing insurance policy
        Task UpdatePolicyAsync(Policy policy);

        // Removes an insurance product from the catalog permanently
        Task DeletePolicyAsync(int PolicyId);

        // Retrieves only the active policy used bv customer and admin to view the details of active policy
        Task<IEnumerable<Policy>> GetActivePoliciesAsync();


        // Retrieves only the Inactive policy admin to view the details of active policy
        Task<IEnumerable<Policy>> GetInActivePoliciesAsync();
    }
}