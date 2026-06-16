using Insurance.Application.DTOs.CustomerPolicyDTO;
using Insurance.Application.DTOs.UserDTO;
using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces
{
    public interface ICustomerPolicyService
    {
        Task<IEnumerable<CustomerPolicyResponseDto>> GetAllCustomerPoliciesAsync();

        Task<CustomerPolicyResponseDto> GetCustomerPolicyByIdAsync(int customerPolicyId);

        Task<IEnumerable<CustomerPolicyResponseDto>> GetByUserIdAsync(int userId);

        Task<IEnumerable<CustomerPolicyResponseDto>> GetByAgentIdAsync(int agentId);

        Task<IEnumerable<AgentCustomerResponseDto>> GetCustomersByAgentIdAsync(int agentId);

        Task AddCustomerPolicyAsync(int verifiedUserId, PurchasePolicyDto dto);

        Task UpdateCustomerPolicyAsync(UpdateCustomerPolicyStatusDto dto);

        Task DeleteCustomerPolicyAsync(int customerPolicyId);

        Task<decimal> CalculateAgentCommissionAsync(int agentId);
        Task RenewPolicyAsync(int verifiedUserId, int expiringCustomerPolicyId);

    }
}
