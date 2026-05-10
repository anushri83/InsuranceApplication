using Insurance.Application.Interfaces;
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;
using Microsoft.Data.SqlClient;

namespace Insurance.Application.Services
{
    public class ClaimService : IClaimService
    {

        private readonly IClaimRepository _claimRepository;
        private readonly ICustomerPolicyRepository _customerPolicyRepository;

        public ClaimService(IClaimRepository claimRepository, ICustomerPolicyRepository customerPolicyRepository)
        {
            _claimRepository = claimRepository;
            _customerPolicyRepository = customerPolicyRepository;
        }

        public async Task<IEnumerable<Claim>> GetAllClaimsAsync()
        {
            try
            {
                var claims = await _claimRepository.GetAllClaimsAsync();
                if(claims == null)
                {
                    return Enumerable.Empty<Claim>();
                }
                return claims;
            }
            catch (Exception)
            {

                throw new Exception("Error occured at service layer"); ;
            }
        }

        public async Task<Claim> GetClaimByIdAsync(int claimId)
        {
            try
            {
                var claim = await _claimRepository.GetClaimByIdAsync(claimId);
                if (claim == null)
                {
                    throw new KeyNotFoundException($"Customer policy with ID {claimId} not found.");
                }
                return claim;
            }
            catch (Exception)
            {

                throw new Exception("Error occured at service layer"); ;
            }
        }
        public async Task AddClaimAsync(Claim claim)
        {
            var customerpolicy = _customerPolicyRepository.GetCustomerPolicyByIdAsync(claim.CustomerPolicyId).Result;
            // Verify the CustomerPolicy exists
            if (customerpolicy == null)
            {
                throw new KeyNotFoundException("Cannot file claim: The associated Customer Policy does not exist.");
            }
            //Only 'Active' policies can have claims filed
            if (customerpolicy.Status != PolicyStatus.Active)
            {
                throw new InvalidOperationException("Cannot file claim: The associated Customer Policy is not active.");
            }
            // Force status to Pending (prevents 'Self-Approval' hacks)
            claim.Status = ClaimStatus.Pending;
            claim.CreatedAt = DateTime.Now;
            await _claimRepository.AddClaimAsync(claim);
        }

        public async Task ApproveClaimAsync(int claimId)
        {
            var claim = _claimRepository.GetClaimByIdAsync(claimId).Result;
            if(claim == null)
            {
                throw new KeyNotFoundException($"Claim with ID {claimId} not found.");
            }
            claim.Status = ClaimStatus.Approved;
            await _claimRepository.UpdateClaimAsync(claim);
        }

        public async Task DeleteClaimAsync(int claimId)
        {
            var claim = _claimRepository.GetClaimByIdAsync(claimId).Result;
            if(claim == null)
            {
                throw new KeyNotFoundException($"Claim with ID {claimId} not found.");
            }
            await _claimRepository.DeleteClaimAsync(claimId);
        }

       

        public async Task RejectClaimAsync(int claimId, string reason)
        {
            var claim = await _claimRepository.GetClaimByIdAsync(claimId);
            if (claim == null)
            {
                throw new KeyNotFoundException("Claim not found.");
            }

            // Logic: Change status to Rejected
            claim.Status = ClaimStatus.Rejected;
            await _claimRepository.UpdateClaimAsync(claim);
        }
    }
}
