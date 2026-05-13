using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces;

public interface IUserService
{
    Task<User> GetUserByIdAsync(int UserId);

    Task<User> GetUserByEmailAsync(string email);

    Task<IEnumerable<User>> GetAllUsersAsync();

    Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);


    Task AddUserAsync(User user);

    Task UpdateUserAsync(User user);

    Task DeleteUserAsync(User user);
}

