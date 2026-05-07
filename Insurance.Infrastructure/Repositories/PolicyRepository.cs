using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient; // Needed for SqlException
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;

public class PolicyRepository : IPolicyRepository
{
    private readonly AppDbContext _context;
    public PolicyRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Policy>> GetAllAsync()
    {
        try
        {
            return await _context.Policies.ToListAsync();
        }
        catch (SqlException ex)
        {
            // The Database server itself is having trouble
            throw new Exception("Technical error: Unable to connect to the database server.", ex);
        }
    }

    public async Task<Policy> GetByIdAsync(int id)
    {
        // FindAsync is usually safe, but we catch nulls in the Service layer
        if (id < 0)
        {
            throw new ArgumentException("Policy ID cannot be negative.");
        }
        var policy =await _context.Policies.FindAsync(id);
        if (policy == null) 
        {
            throw new Exception("Policy not found."); // Let the Service layer handle this as a "not found" case
        }
        return policy;
    }

    public async Task AddAsync(Policy policy)
    {
        try
        {
            await _context.Policies.AddAsync(policy);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // This happens if a Database Constraint is broken 
            // (e.g., trying to add a policy with an ID that already exists)
            throw new InvalidOperationException("Could not save policy. Please check if the data violates database rules.", ex);
        }
        catch (Exception ex)
        {
            // Generic fallback for anything else
            throw new Exception("An unexpected error occurred while saving the policy.", ex);
        }
    }

    public Task<IEnumerable<Policy>> GetAllPoliciesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Policy> GetPolicyByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddPolicyAsync(Policy policy)
    {
        throw new NotImplementedException();
    }
}