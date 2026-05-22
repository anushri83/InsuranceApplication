using Insurance.Application.DTOs.CustomerPolicyDTO;
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

        public async Task<IEnumerable<CustomerPolicyResponseDto>> GetAllCustomerPoliciesAsync()
        {
            try
            {
                var customerPolicies = await _customerPolicyRepository.GetAllCustomerPoliciesAsync();
                if (customerPolicies == null)
                {
                    throw new KeyNotFoundException($"Customer policies not found.");
                }

                // 💡 FIXED: Changed MapToResponseDto to MapToResponseDtoList
                return MapToResponseDtoList(customerPolicies);
            }
            catch (KeyNotFoundException)
            {
                throw; // Let our specific "Not Found" message pass through cleanly
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred at service layer", ex);
            }
        }

        public async Task<CustomerPolicyResponseDto> GetCustomerPolicyByIdAsync(int customerPolicyId)
        {
            try
            {
                var customerPolicy = await _customerPolicyRepository.GetCustomerPolicyByIdAsync(customerPolicyId);
                if (customerPolicy == null)
                {
                    throw new KeyNotFoundException($"Customer policy with ID {customerPolicyId} not found.");
                }
                return MapToResponseDto(customerPolicy);
            }
            catch (Exception)
            {
                throw new Exception("Error occured at service layer");
            }
            
        }

        public async Task<IEnumerable<CustomerPolicyResponseDto>> GetByUserIdAsync(int userId)
        {
            try
            {
                var customerPolicies = await _customerPolicyRepository.GetByUserIdAsync(userId);
                if (customerPolicies == null)
                {
                    throw new KeyNotFoundException($"Customer policies with User ID {userId} not found.");
                }
                return MapToResponseDtoList(customerPolicies);
            }
            catch (Exception)
            {
                throw new Exception("Error occured at service layer");
            }
            
        }

        public async Task<IEnumerable<CustomerPolicyResponseDto>> GetByAgentIdAsync(int agentId)
        {
            try
            {
                var customerPolicies = await _customerPolicyRepository.GetByAgentIdAsync(agentId);
                if(customerPolicies == null)
                {
                    throw new KeyNotFoundException($"Customer policies with Agent ID {agentId} not found.");
                }
                return MapToResponseDtoList(customerPolicies);
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
                    throw new KeyNotFoundException($"Customer policies for Agent ID {agentId} not found.");
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


        public async Task AddCustomerPolicyAsync(PurchasePolicyDto dto)
        {
            try
            {
                var policy = await _policyRepository.GetPolicyByPolicyIdAsync(dto.PolicyId);

                if (policy == null)
                {
                    throw new KeyNotFoundException($"Policy with ID {dto.PolicyId} does not exist.");
                }

                var customerPolicy = new CustomerPolicy
                {
                    CustomerPolicyId = 0,
                    UserId = dto.UserId,
                    PolicyId = dto.PolicyId,
                    AgentId = dto.AgentId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(policy.DurationInMonth),
                    Status = CustomerPolicyStatus.Active,
                    CreatedAt = DateTime.Now
                };

                await _customerPolicyRepository.AddCustomerPolicyAsync(customerPolicy);

            }
            catch (Exception)
            {
                throw new Exception("Error occured at service layer");
            }
        }

        public async Task UpdateCustomerPolicyAsync(UpdateCustomerPolicyStatusDto dto)
        {
            try
            {
                var customerPolicy = await _customerPolicyRepository.GetCustomerPolicyByIdAsync(dto.CustomerPolicyId);
                if (customerPolicy == null)
                {
                    throw new KeyNotFoundException($"Customer policy with ID {dto.CustomerPolicyId} not found.");
                }
                if (dto == null)
                {
                    throw new KeyNotFoundException("Customer policy data cannot be null.");
                }

                var updatepolicy = new CustomerPolicy
                {
                    CustomerPolicyId = dto.CustomerPolicyId,
                    UserId = customerPolicy.UserId,
                    PolicyId = customerPolicy.PolicyId,
                    AgentId = customerPolicy.AgentId,
                    StartDate = customerPolicy.StartDate,
                    EndDate = customerPolicy.EndDate,
                    Status = (CustomerPolicyStatus)dto.Status,
                    CreatedAt = customerPolicy.CreatedAt,
                    UpdatedAt = DateTime.Now
                };
                await _customerPolicyRepository.UpdateCustomerPolicyAsync(updatepolicy);
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

        private CustomerPolicyResponseDto MapToResponseDto(CustomerPolicy cp)
        {
            return new CustomerPolicyResponseDto
            {
                CustomerPolicyId = cp.CustomerPolicyId,
                UserId = cp.UserId,
                CustomerName = cp.User?.Name ?? "Unknown Customer", // Handle null User reference cp.User?.Name  grabs the name if the user exists, and ?? "Unknown Customer" gives "uunknown customer" they don't.
                PolicyId = cp.PolicyId,
                PolicyName = cp.Policy?.PolicyName ?? "Unknown Plan",
                PremiumAmount = cp.Policy?.PremiumAmount ?? 0,
                AgentName = cp.Agent != null ? cp.Agent.Name : "Direct Online", // If there's an agent, show their name; otherwise, it's a direct online purchase.
                StartDate = cp.StartDate,
                EndDate = cp.EndDate,
                Status = cp.Status.ToString()
            };
        }

        //Converts a LIST of database rows into a LIST of clean DTOs .
        private IEnumerable<CustomerPolicyResponseDto> MapToResponseDtoList(IEnumerable<CustomerPolicy> records)
        {
            //  If the list is completely missing, return an empty list [] instead of crashing.
            if (records == null) return Enumerable.Empty<CustomerPolicyResponseDto>();

            // Loop through every single row, convert it using our single row mapper, and save it as a list.
            return records.Select(MapToResponseDto).ToList(); //select acts as a for each loop 
        }



    }
}
