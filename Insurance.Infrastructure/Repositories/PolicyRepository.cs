using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

public class PolicyRepository : IPolicyRepository
{
    private readonly AppDbContext _context;
    public PolicyRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
        => await _context.Policies.ToListAsync();

    public async Task<Policy> GetPolicyByIdAsync(int id)
        => await _context.Policies.FindAsync(id);

    public async Task AddPolicyAsync(Policy policy)
    {
        await _context.Policies.AddAsync(policy);
        await _context.SaveChangesAsync();
    }
}