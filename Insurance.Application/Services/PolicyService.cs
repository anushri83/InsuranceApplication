
using Insurance.Application.Interfaces;
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;

namespace Insurance.Application.Services;

public class PolicyService : IPolicyService
{
    private readonly IPolicyRepository _repository;

    public PolicyService(IPolicyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
    {
        try
        {
            var policies = await _repository.GetAllPolicyAsync();

            if (policies == null || !policies.Any())
            {
                return Enumerable.Empty<Policy>();
            }

            return policies;
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while fetching all policies.", ex);
        }
    }

    public async Task<Policy> GetPolicyByPolicyIdAsync(int policyId)
    {
        try
        {
            if (policyId <= 0)
            {
                throw new ArgumentException("Invalid Policy ID.");
            }

            var policy = await _repository.GetPolicyByPolicyIdAsync(policyId);

            if (policy == null)
            {
                throw new KeyNotFoundException(
                    $"No policy found with ID {policyId}.");
            }

            return policy;
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while fetching policy by ID.", ex);
        }
    }

    public async Task CreatePolicyAsync(Policy policy)
    {
        try
        {
            if (policy == null)
            {
                throw new ArgumentNullException(
                    nameof(policy),
                    "Policy data cannot be null.");
            }

            if (policy.PremiumAmount <= 0)
            {
                throw new InvalidOperationException(
                    "Premium amount must be greater than 0.");
            }

            await _repository.CreatePolicyAsync(policy);
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while creating policy.", ex);
        }
    }

    public async Task UpdatePolicyAsync(Policy policy)
    {
        try
        {
            if (policy == null)
            {
                throw new ArgumentNullException(
                    nameof(policy),
                    "Policy data cannot be null.");
            }

            if (policy.PremiumAmount <= 0)
            {
                throw new InvalidOperationException(
                    "Premium amount must be greater than 0.");
            }

            await _repository.UpdatePolicyAsync(policy);
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while updating policy.", ex);
        }
    }

    public async Task DeletePolicyAsync(int policyId)
    {
        try
        {
            if (policyId <= 0)
            {
                throw new ArgumentException(
                    "Policy ID must be greater than 0.");
            }

            var policy =
                await _repository.GetPolicyByPolicyIdAsync(policyId);

            if (policy == null)
            {
                throw new KeyNotFoundException(
                    $"No policy found with ID {policyId}.");
            }

            await _repository.DeletePolicyAsync(policyId);
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while deleting policy.", ex);
        }
    }

    public async Task<IEnumerable<Policy>> GetActivePoliciesAsync()
    {
        try
        {
            var policies = await _repository.GetActivePoliciesAsync();

            if (policies == null || !policies.Any())
            {
                return Enumerable.Empty<Policy>();
            }

            return policies;
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while fetching active policies.", ex);
        }
    }

    public async Task<IEnumerable<Policy>> GetInActivePoliciesAsync()
    {
        try
        {
            var policies = await _repository.GetInActivePoliciesAsync();

            if (policies == null || !policies.Any())
            {
                return Enumerable.Empty<Policy>();
            }

            return policies;
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while fetching inactive policies.", ex);
        }
    }
}