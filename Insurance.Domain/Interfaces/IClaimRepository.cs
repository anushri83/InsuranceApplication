using Insurance.Domain.Models;

namespace Insurance.Domain.Interfaces
{
    public interface IClaimRepository
    {
        // Retrieves every claim recorded in the system for administrative overview
        Task<IEnumerable<Claim>> GetAllClaimsAsync();

        // Fetches a single claim detail including the policy and user information
        Task<Claim> GetClaimByClaimIdAsync(int claimId);

        // Retrieves all claims belonging to a specific customer by joining through their policies
        Task<IEnumerable<Claim>> GetClaimsByUserIdAsync(int userId);

        // Adds a new claim record to the database
        Task AddClaimAsync(Claim claim);

        // Updates an existing claim for status changes like approval or rejection
        Task UpdateClaimAsync(Claim claim);

        // Permanently removes a claim record from the system
        Task DeleteClaimAsync(int claimId);

        
    }
}