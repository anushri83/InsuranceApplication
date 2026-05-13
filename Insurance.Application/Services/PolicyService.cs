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

    public async Task<IEnumerable<Policy>> GetAllPolicyAsync()
    {
        try
        {
            var policies = await _repository.GetAllPolicyAsync();

            // If the list is empty, that's not a crash, but we can handle it
            if (policies == null || !policies.Any())
            {
                // We can return an empty list or throw a specific error
                return Enumerable.Empty<Policy>();
            }
            return policies;
        }

        catch (Exception ex)
        {
            throw new Exception("Error occured at service layer", ex); 
        }
    }

    public async Task<Policy> GetPolicyByPolicyIdAsync(int PolicyId)
    {
        try 
        {
            if (PolicyId < 0)
            { 
                throw new ArgumentException("Id Invalid");
            }
            var policy = await _repository.GetPolicyByPolicyIdAsync(PolicyId);
            if (policy == null)
            {
                throw new KeyNotFoundException($"No policy with ID {PolicyId}");
            }
            return policy;
        }
        catch (Exception)
        {
            throw new Exception("Error occured at service layer");
        }

    }
    public async Task AddPolicyAsync(Policy policy)
    {
        try
        {
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy), "Policy data cannot be null.");
            }
            if (policy.PremiumAmount <= 0)
            {
                throw new Exception("Premium Amount cannot be 0");
            }
            await _repository.AddPolicyAsync(policy);
        }
        catch (Exception)
        {
            throw new Exception("Error occured at service layer");
        }
        
    }

    public async Task UpdatePolicyAsync(Policy policy)
    {
        try
        {
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy), "Policy data cannot be null.");
            }
            if (policy.PremiumAmount <= 0)
            {
                throw new Exception("Premium Amount cannot be 0");
            }
            await _repository.UpdatePolicyAsync(policy);
        }
        catch (Exception)
        {
            throw new Exception("Error occured at service layer");
        }
    }

    public async Task DeletePolicyAsync(int PolicyId)
    {
        try
        {
            if (PolicyId <= 0)
            {
                throw new Exception("PolicyId cannot be less than 0");
            }
            await _repository.DeletePolicyAsync(PolicyId);
        }
        catch (Exception)
        {
            throw new Exception("Error occured at service layer");
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
            throw new Exception("Error occured at service layer", ex);
        }
    }

    public async Task<IEnumerable<Policy>> GetInActivePoliciesAsync()
    {
        try
        {
            var policies =  await _repository.GetInActivePoliciesAsync();
            if(policies == null || !policies.Any())
            {
                return Enumerable.Empty<Policy>();
            }
            return policies;
        }
        catch (Exception ex)
        {
            throw new Exception("Error occured at service layer", ex);
        }
    }
}
