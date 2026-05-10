using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces
{
    public interface IClaimService
    {
        Task<IEnumerable<Claim>> GetAllClaimsAsync();
        Task<Claim> GetClaimByIdAsync(int claimId);
        Task AddClaimAsync(Claim claim); // For Customers to file a new claim
        Task ApproveClaimAsync(int claimId); // Admin Action
        Task RejectClaimAsync(int claimId ,string reason); // Admin Action
        Task DeleteClaimAsync(int claimId);
    }
}