using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient; // Needed for SqlException
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;

namespace Insurance.Infrastructure.Repositories
{
    public class CustomerPolicyRepository : ICustomerPolicyRepository
    {
        private readonly AppDbContext _context;

        public CustomerPolicyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerPolicy>> GetAllCustomerPoliciesAsync()
        {
            try
            {
                 return await _context.CustomerPolicies.Include(cp => cp.User).Include(cp=> cp.Policy).ToListAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while retrieving customer policies.", ex);
            }
        }

        public async Task<CustomerPolicy> GetCustomerPolicyByIdAsync(int customerPolicyId)
        {
            try
            {
                // Use FirstOrDefaultAsync instead of FindAsync when using Include
                return await _context.CustomerPolicies.Include(cp => cp.User).Include(cp => cp.Policy)
                    .FirstOrDefaultAsync(cp => cp.CustomerPolicyId == customerPolicyId);

            }
            catch (SqlException ex)
            {
                throw new Exception("Database error while retrieving customer policy.", ex);
            }
        }

        // For Customer: "Show me the policies I own"       
        public async Task<IEnumerable<CustomerPolicy>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await _context.CustomerPolicies
                .Include(cp => cp.Policy)
                .Include(cp => cp.Agent) // To see which agent is helping them
                .Where(cp => cp.UserId == userId)
                .ToListAsync();

            }
            catch (SqlException ex)
            {
                throw new Exception("Database error while retrieving customer policy.", ex);
            }

        }

        public async Task<IEnumerable<User>> GetCustomersByAgentIdAsync(int agentId)
        {
            return await _context.CustomerPolicies.Include(cp => cp.User).Where(cp => cp.AgentId == agentId)
                .Select(cp => cp.User).Distinct().ToListAsync();

        }

        public async Task AddCustomerPolicyAsync(CustomerPolicy customerPolicy)
        {
            try
            {
                await _context.CustomerPolicies.AddAsync(customerPolicy);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Could not save policy. Please check if the data violates database rules.", ex);
            }
               
        }

        public async Task UpdateCustomerPolicyAsync(CustomerPolicy customerPolicy)
        {
            try
            {
                var localInstance = _context.CustomerPolicies.Local.FirstOrDefault(entry => entry.CustomerPolicyId == customerPolicy.CustomerPolicyId);
                if (localInstance != null)
                {
                    _context.Entry(localInstance).State = EntityState.Detached;
                }
                _context.CustomerPolicies.Update(customerPolicy);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Could not update policy. Please check if the data violates database rules.", ex);
            }
            
        }

        public async Task DeleteCustomerPolicyAsync(int customerPolicyId)
        {
            var customerpolicy = await GetCustomerPolicyByIdAsync(customerPolicyId);
            if (customerpolicy != null)
            {
                try
                {
                    var localInstance = _context.CustomerPolicies.Local.FirstOrDefault(entry => entry.CustomerPolicyId == customerPolicyId);
                    if (localInstance != null)
                    {
                        _context.Entry(localInstance).State = EntityState.Detached;
                    }
                    _context.CustomerPolicies.Remove(customerpolicy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    throw new InvalidOperationException("Could not delete policy. Please check if the data violates database rules.", ex);
                }
               
            }

        }

        // For Agent: "Show me all policies I have sold"
        public async Task<IEnumerable<CustomerPolicy>> GetByAgentIdAsync(int agentId)
        {
            return await _context.CustomerPolicies
                .Include(cp => cp.User)   // The Customer
                .Include(cp => cp.Policy) // The Product
                .Where(cp => cp.AgentId == agentId)
                .ToListAsync();
        }


    }
}
