using Insurance.Domain.Models;

namespace Insurance.Domain.Interfaces
{
    public interface IUserRepository
    {
        // Fetches a single user's profile information using their unique ID
        Task<User?> GetUserByIdAsync(int userId);

        // Retrieves a user record based on their email address, primarily used for login and unique checks
        Task<User?> GetUserByEmailAsync(string email);

        // Returns a complete list of all users registered in the system for administrative management
        Task<IEnumerable<User>> GetAllUsersAsync();

        // Filters the user directory to return only specific groups, such as a list of all Agents or all Customers
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);

        // Inserts a new user record into the database during registration or onboarding
        Task RegisterUserAsync(User user);

        // Updates personal details, roles, or status for an existing user
        Task UpdateUserAsync(User user);

        // Permanently removes a user record from the system
        Task DeleteUserAsync(int userId);

        
    }
}