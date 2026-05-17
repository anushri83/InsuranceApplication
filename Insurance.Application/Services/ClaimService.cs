using Insurance.Application.Interfaces;
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;

namespace Insurance.Application.Services
{
    public class ClaimService : IClaimService
    {

        private readonly IClaimRepository _claimRepository;
        private readonly ICustomerPolicyRepository _customerPolicyRepository;
        private readonly IPolicyRepository _policyRepository;

        public ClaimService(IClaimRepository claimRepository, ICustomerPolicyRepository customerPolicyRepository, IPolicyRepository policyRepository)
        {
            _claimRepository = claimRepository;
            _customerPolicyRepository = customerPolicyRepository;
            _policyRepository = policyRepository;
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


        public async Task<Claim> GetClaimByClaimIdAsync(int claimId)
        {
            try
            {
                var claim = await _claimRepository.GetClaimByClaimIdAsync(claimId);
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

        public async Task<IEnumerable<Claim>> GetClaimsByUserIdAsync(int UserId)
        {
            try
            {
                var claims =  await _claimRepository.GetClaimsByUserIdAsync(UserId);
                if (claims == null)
                {
                    throw new KeyNotFoundException($"Claim with ID {UserId} not found.");
                }
                return claims;
            }
            catch (Exception)
            {
                throw new Exception("Error occured at service layer"); 
            }
        }
       
        public async Task AddClaimAsync(Claim claim)
        {
            try
            {
                var customerpolicy = await _customerPolicyRepository.GetCustomerPolicyByIdAsync(claim.CustomerPolicyId);
                if (customerpolicy == null)
                {
                    throw new KeyNotFoundException("Cannot file claim: The associated Customer Policy does not exist.");
                }
                if (customerpolicy.Status != CustomerPolicyStatus.Active)
                {
                    throw new InvalidOperationException("Cannot file claim: The associated Customer Policy is not active.");
                }
                // Check 1: Date Validation
                if (DateTime.Now < customerpolicy.StartDate || DateTime.Now > customerpolicy.EndDate)
                {
                    throw new InvalidOperationException("Cannot file a claim on an inactive or expired policy.");
                }

                // Check 2: Amount Validation
                var policyDetails = await _policyRepository.GetPolicyByPolicyIdAsync(customerpolicy.PolicyId);
                if (policyDetails == null)
                {
                    throw new KeyNotFoundException("The associated Policy does not exist.");
                }

                if (claim.ClaimAmount > policyDetails.PremiumAmount * 10) // Example rule: Max claim cap
                {
                    throw new InvalidOperationException("Claim amount exceeds the policy coverage limit.");
                }

                // Force status to Pending (prevents 'Self-Approval' hacks)
                claim.Status = ClaimStatus.Pending;
                claim.CreatedAt = DateTime.Now;
                await _claimRepository.AddClaimAsync(claim);
            }
            catch (Exception)
            {
                throw new Exception("Error occured at service layer");
            }

        }

       

        public async Task DeleteClaimAsync(int claimId)
        {
            var claim = _claimRepository.GetClaimByClaimIdAsync(claimId).Result;
            if(claim == null)
            {
                throw new KeyNotFoundException($"Claim with ID {claimId} not found.");
            }
            await _claimRepository.DeleteClaimAsync(claimId);
        }

        public async Task ApproveClaimAsync(int claimId)
        {
            var claim = await _claimRepository.GetClaimByClaimIdAsync(claimId);
            if (claim == null)
            {
                throw new KeyNotFoundException($"Claim with ID {claimId} not found.");
            }
            claim.Status = ClaimStatus.Approved;
            await _claimRepository.UpdateClaimAsync(claim);
        }

        public async Task RejectClaimAsync(int claimId, string reason)
        {
            var claim = await _claimRepository.GetClaimByClaimIdAsync(claimId);
            if (claim == null)
            {
                throw new KeyNotFoundException("Claim not found.");
            }

            claim.Status = ClaimStatus.Rejected;
            await _claimRepository.UpdateClaimAsync(claim);
        }


    }
}
