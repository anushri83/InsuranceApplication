using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;

namespace Insurance.Infrastructure.Repositories
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly AppDbContext _context;

        public ClaimRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Claim>> GetAllClaimsAsync()
        {
            try
            {
                // Joins Claim -> CustomerPolicy -> Policy AND User
                return await _context.Claims
                    .Include(c => c.customerPolicy)
                        .ThenInclude(cp => cp.Policy)
                    .Include(c => c.customerPolicy)
                        .ThenInclude(cp => cp.User)
                    .ToListAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Technical error: Could not retrieve claims from database.", ex);
            }
        }

        public async Task<Claim?> GetClaimByIdAsync(int claimId)
        {
            try
            {
                // We use FirstOrDefaultAsync because FindAsync does not support .Include()
                return await _context.Claims
                .Include(c => c.customerPolicy)
                    .ThenInclude(cp => cp.Policy)
                .Include(c => c.customerPolicy)
                    .ThenInclude(cp => cp.User)
                .FirstOrDefaultAsync(c => c.ClaimId == claimId);
            }
            catch (SqlException ex)
            {
                throw new Exception("Technical error: Could not retrieve claim from database.", ex);
            }

        }

        public async Task AddClaimAsync(Claim claim)
        {
            try
            {
                await _context.Claims.AddAsync(claim);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Database error: Could not save the claim.", ex);
            }
        }

        public async Task UpdateClaimAsync(Claim claim)
        {
            try
            {
                // Used by Service for Approve/Reject logic
                _context.Claims.Update(claim);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Database error: Could not update the claim.", ex);
            }
            
        }

        public async Task DeleteClaimAsync(int claimId)
        {
            try
            {
                var claim = await _context.Claims.FindAsync(claimId);
                if (claim != null)
                {
                    _context.Claims.Remove(claim);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Database error: Could not delete the claim.", ex);
            }
            
        }
    }
}