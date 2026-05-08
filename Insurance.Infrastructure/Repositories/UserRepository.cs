using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient; // Needed for SqlException
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;


namespace Insurance.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            // Simply return the user or null. Let the caller handle the "NotFound" logic.
            return await _context.Users.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task AddUser(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) // More specific error for Database issues
            {
                throw new Exception("Error saving user to the database.", ex);
            }
        }

        public async Task UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) // More specific error for Database issues
            {
                throw new Exception("Error saving user to the database.", ex);
            }
            
        }

        public async Task DeleteUser(User user)
        {
            // You MUST call Remove before SaveChanges
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
