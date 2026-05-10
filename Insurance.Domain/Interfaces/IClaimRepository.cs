using Insurance.Domain.Models;

namespace Insurance.Domain.Interfaces
{
    public interface IClaimRepository
    {
        Task<IEnumerable<Claim>> GetAllClaimsAsync();
        Task<Claim> GetClaimByIdAsync(int claimId);
        Task AddClaimAsync(Claim claim);
        Task UpdateClaimAsync(Claim claim);
        Task DeleteClaimAsync(int claimId);
    }
}