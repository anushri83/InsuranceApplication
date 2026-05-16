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

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int UserId)
        {
            return await _context.Users.FirstOrDefaultAsync(e => e.UserId == UserId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _context.Users.Where(e => e.Role == role).ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error saving user to the database.", ex);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                // Check if EF Core tracker already holds an instance with this UserId
                var localInstance = _context.Users
                    .Local
                    .FirstOrDefault(entry => entry.UserId == user.UserId);

                // If a tracked instance exists, detach it so it stops blocking our update object
                if (localInstance != null)
                {
                    _context.Entry(localInstance).State = EntityState.Detached;
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error updating user to the database.", ex);
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                try
                {
                    // Check if EF Core tracker already holds an instance with this UserId
                    var localInstance = _context.Users
                        .Local
                        .FirstOrDefault(entry => entry.UserId == user.UserId);

                    // If a tracked instance exists, detach it so it stops blocking our update object
                    if (localInstance != null)
                    {
                        _context.Entry(localInstance).State = EntityState.Detached;
                    }
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    throw new Exception("Error deleting user from the database.", ex);
                }       
            }
           
        }

    }
}