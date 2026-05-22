using Insurance.Application.DTOs.ClaimDTO;
using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces
{
    public interface IClaimService
    {
        Task<IEnumerable<ClaimResponseDto>> GetAllClaimsAsync();
        Task<ClaimResponseDto> GetClaimByClaimIdAsync(int claimId);
        Task<IEnumerable<ClaimResponseDto>> GetClaimsByUserIdAsync(int userId);
        Task AddClaimAsync(CreateClaimDto dto); // For Customers to file a new claim
        Task UpdateClaimAsync(UpdateClaimDto dto);
        Task ApproveClaimAsync(int claimId); // Admin Action
        Task RejectClaimAsync(int claimId ,string reason); // Admin Action
        Task DeleteClaimAsync(int claimId);
    }
}