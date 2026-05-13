using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient; // Needed for SqlException
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;

namespace Insurance.Infrastructure.Repositories;
public class PolicyRepository : IPolicyRepository
{
    private readonly AppDbContext _context;
    public PolicyRepository(AppDbContext context) 
    {
        _context = context; 
    }

    public async Task<IEnumerable<Policy>> GetAllPolicyAsync()
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


    public async Task<Policy?> GetPolicyByPolicyIdAsync(int policyId)
    {
        try
        {
            return await _context.Policies.FindAsync(policyId);
        }
        catch (SqlException ex)
        {
            throw new Exception("Database error while retrieving policy.", ex);
        }
    }

    public async Task AddPolicyAsync(Policy policy)
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

    public async Task UpdatePolicyAsync(Policy policy)
    {
        try
        {
            _context.Policies.Update(policy);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Could not save policy. Please check if the data violates database rules.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while saving the policy.", ex);
        }
    }

    public async Task DeletePolicyAsync(int PolicyId)
    {
        try
        {
            var policy = await _context.Policies.FindAsync(PolicyId);
            if (policy != null)
            {
                _context.Policies.Remove(policy);
                await _context.SaveChangesAsync();
            }
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Could not delete policy. Please check if the data violates database rules.", ex);
        }

    }

    public async Task<IEnumerable<Policy>> GetActivePoliciesAsync()
    {
        try
        {
            return await _context.Policies.Where(p => p.Status == PolicyStatus.Active).ToListAsync();

        }
        catch (SqlException ex)
        {
            throw new Exception("Database error while retrieving policy.", ex);
        }
    }

    public async Task<IEnumerable<Policy>> GetInActivePoliciesAsync()
    {
        try
        {
            return await _context.Policies.Where(p => p.Status== PolicyStatus.Inactive).ToListAsync();

        }
        catch (SqlException ex)
        {
            throw new Exception("Database error while retrieving policy.", ex);
        }
    }
}
