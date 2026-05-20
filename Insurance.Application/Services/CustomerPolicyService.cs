using Insurance.Application.DTOs.UserDTO;
using Insurance.Application.Interfaces;
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Repositories;
using System.Security.Claims;

namespace Insurance.Application.Services
{
    public class CustomerPolicyService: ICustomerPolicyService
    {
        private readonly ICustomerPolicyRepository _customerPolicyRepository;
        private readonly IPolicyRepository _policyRepository;

        public CustomerPolicyService(ICustomerPolicyRepository customerPolicyRepository , IPolicyRepository policyRepository)
        {
            _customerPolicyRepository = customerPolicyRepository;
            _policyRepository = policyRepository;
        }

        public async Task<IEnumerable<CustomerPolicy>> GetAllCustomerPoliciesAsync() 
        {
            try
            {
                var customerPolicies = await _customerPolicyRepository.GetAllCustomerPoliciesAsync();
                if (customerPolicies == null)
                {
                    return Enumerable.Empty<CustomerPolicy>();
                }
                return customerPolicies;
            }
            catch (Exception)
            {

                throw new Exception("Error occured at service layer"); ;
            }
            
        }
        public async Task<CustomerPolicy> GetCustomerPolicyByIdAsync(int customerPolicyId)
        {
            try
            {
                var customerPolicy = await _customerPolicyRepository.GetCustomerPolicyByIdAsync(customerPolicyId);
                if (customerPolicy == null)
                {
                    throw new KeyNotFoundException($"Customer policy with ID {customerPolicyId} not found.");
                }
                return customerPolicy;
            }
            catch (Exception)
            {
                throw new Exception("Error occured at service layer");
            }
            
        }
        public async Task<IEnumerable<CustomerPolicy>> GetByUserIdAsync(int userId)
        {
            try
            {
                var customerPolicies = await _customerPolicyRepository.GetByUserIdAsync(userId);
                if (customerPolicies == null)
                {
                    return Enumerable.Empty<CustomerPolicy>();
                }
                return customerPolicies;
            }
            catch (Exception)
            {
                throw new Exception("Error occured at service layer");
            }
            
        }
        public async Task<IEnumerable<CustomerPolicy>> GetByAgentIdAsync(int agentId)
        {
            try
            {
                var customerPolicies = await _customerPolicyRepository.GetByAgentIdAsync(agentId);
                if(customerPolicies == null)
                {
                    return Enumerable.Empty<CustomerPolicy>();
                }
                return customerPolicies;
            }
            catch (Exception)
            {
                throw new Exception("Error occured at service layer");
            }
        }

        public async Task<IEnumerable<AgentCustomerResponseDto>> GetCustomersByAgentIdAsync(int agentId)
        {
            try
            {
                var customers = await _customerPolicyRepository.GetCustomersByAgentIdAsync(agentId);
                if (customers == null)
                {
                    return Enumerable.Empty<AgentCustomerResponseDto>(); 
                }

                var dtoList = new List<AgentCustomerResponseDto>();

                foreach (var customer in customers)
                {
                    dtoList.Add(new AgentCustomerResponseDto
                    {
                        UserId = customer.UserId,
                        Name = customer.Name,
                        Email = customer.Email,
                        PhoneNumber = customer.PhoneNumber,
                        City = customer.City,
                        IsActive = customer.IsActive,
                    });
                }

                return dtoList;
            }
            catch (Exception)
            {
                throw new Exception("Error occurred at service layer");
            }
        }


        public async Task AddCustomerPolicyAsync(CustomerPolicy customerPolicy)
        {
            try
            {
                var customerpolicy = await _customerPolicyRepository.GetCustomerPolicyByIdAsync(customerPolicy.CustomerPolicyId);
                var policy = await _policyRepository.GetPolicyByPolicyIdAsync(customerPolicy.PolicyId);

                if(policy == null)
                {
                    throw new Exception("Policy does not exist.");
                }
                  
                if (customerpolicy != null)
                {
                    throw new KeyNotFoundException(" The associated Customer Policy already exist.");
                }
                customerPolicy.StartDate = DateTime.Now;
                // Automatically add months based on the policy rules
                customerPolicy.EndDate = customerPolicy.StartDate.AddMonths(policy.DurationInMonth);
                customerPolicy.Status = CustomerPolicyStatus.Active;
                await _customerPolicyRepository.AddCustomerPolicyAsync(customerPolicy);

            }
            catch (Exception)
            {
                throw new Exception("Error occured at service layer");
            }
            
            
        }
        public async Task UpdateCustomerPolicyAsync(CustomerPolicy customerPolicy)
        {
            try
            {
                if (customerPolicy == null)
                {
                    throw new KeyNotFoundException("Customer policy data cannot be null.");
                }
                await _customerPolicyRepository.UpdateCustomerPolicyAsync(customerPolicy);
            }
            catch (Exception)
            {
                throw new Exception("Error occured at service layer");
            }
            
        }
        public async Task DeleteCustomerPolicyAsync(int customerPolicyId)
        {
            try
            {
                if (customerPolicyId == 0)
                {
                    throw new KeyNotFoundException("Customer policy ID cannot be zero.");
                }
                await _customerPolicyRepository.DeleteCustomerPolicyAsync(customerPolicyId);
            }
           catch (Exception)
            {
                throw new Exception("Error occured at service layer");
            }
        }

        public async Task<decimal> CalculateAgentCommissionAsync(int agentId)
        {
            // 1. Fetch all policies sold by this specific agent
            var activeSales = await GetByAgentIdAsync(agentId);

            decimal totalCommission = 0;

            foreach (var sale in activeSales)
            {
                var policy = await _policyRepository.GetPolicyByPolicyIdAsync(sale.PolicyId);

                if (policy == null)
                {
                    throw new KeyNotFoundException(" policy ID does not exist .");
                }
                // Apply a flat 10% commission rule on the price
                totalCommission += (policy.PremiumAmount * 0.10m);
            }

            return totalCommission;
        }


    }
}
