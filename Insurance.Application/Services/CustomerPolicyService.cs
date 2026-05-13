using Insurance.Application.Interfaces;
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;

namespace Insurance.Application.Services
{
    public class CustomerPolicyService: ICustomerPolicyService
    {
        private readonly ICustomerPolicyRepository _customerPolicyRepository;

        public CustomerPolicyService(ICustomerPolicyRepository customerPolicyRepository)
        {
            _customerPolicyRepository = customerPolicyRepository;
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

        public async Task<IEnumerable<User>> GetCustomersByAgentIdAsync(int agentId)
        {
            try
            {
                var customers = await _customerPolicyRepository.GetCustomersByAgentIdAsync(agentId);
                if(customers == null)
                {
                    return Enumerable.Empty<User>();
                }
                return customers;
            }
            catch (Exception)
            {
                throw new Exception("Error occured at service layer");
            }
        }


        public async Task AddCustomerPolicyAsync(CustomerPolicy customerPolicy)
        {
            try
            {
                if (customerPolicy == null)
                {
                    throw new KeyNotFoundException("Customer policy data cannot be null.");
                }
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

      
    }
}
