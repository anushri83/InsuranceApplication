using Insurance.Application.DTOs.UserDTO;
using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces;

public interface IUserService
{
    Task<User> GetUserByIdAsync(int UserId);

    Task<User> GetUserByEmailAsync(string email);

    Task<IEnumerable<User>> GetAllUsersAsync();

    Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);

    Task RegisterCustomerAsync(CreateCustomerDto dto);

    Task RegisterAgentAsync(CreateAgentDto dto);

    Task RegisterAdminAsync(CreateAdminDto dto);

    Task UpdateUserAsync(UpdateUserDto dto);

    Task ChangePasswordAsync(ChangePasswordDto dto);

    Task DeleteUserAsync(int userId);
}

