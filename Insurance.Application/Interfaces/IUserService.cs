using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces;

public interface IUserService
{
    public Task<User> GetUserByIdAsync(int UserId);

    public Task<User> GetUserByEmailAsync(string email);

    public Task AddUserAsync(User user);

    public Task UpdateUserAsync(User user);

    public Task DeleteUserAsync(User user);
}
