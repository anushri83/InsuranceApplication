using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces
{
    public interface IClaimService
    {
        Task<IEnumerable<Claim>> GetAllClaimsAsync();
        Task<Claim> GetClaimByClaimIdAsync(int claimId);
        Task<IEnumerable<Claim>> GetClaimsByUserIdAsync(int UserId);
        Task AddClaimAsync(Claim claim); // For Customers to file a new claim
        Task ApproveClaimAsync(int claimId); // Admin Action
        Task RejectClaimAsync(int claimId ,string reason); // Admin Action
        Task DeleteClaimAsync(int claimId);
    }
}