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
        return await _repository.GetAllPoliciesAsync();
    }

    public async Task<Policy> GetPolicyByIdAsync(int id)
    {
        return await _repository.GetPolicyByIdAsync(id);
    }
    public async Task AddPolicyAsync(Policy policy)
    {
        if (policy.PremiumAmount <= 0)
        {
            throw new Exception("Premium Amount cannot be 0");
        }
        await _repository.AddPolicyAsync(policy);
    }
}
