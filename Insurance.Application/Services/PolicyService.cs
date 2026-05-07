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

    public async Task<IEnumerable<Policy>> GetPoliciesAsync()
    {
        try
        {
            var policies = await _repository.GetAllPoliciesAsync();

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
            throw;
        }
    }

    public async Task<Policy> GetPolicyByIdAsync(int id)
    {

        if (id < 0)
        {
            throw new ArgumentException("Id Invalid");
        }
        var policy = await _repository.GetPolicyByIdAsync(id);

        if (policy == null)
        {
            throw new KeyNotFoundException($"No policy with {id}");
        }
        return policy;
    }

    public async Task AddPolicyAsync(Policy policy)
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
}
