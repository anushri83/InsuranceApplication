using Insurance.Application.Interfaces;
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(int UserId)
        {
            var user = await _userRepository.GetUserByIdAsync(UserId);
            if (user == null)
            { 
                throw new KeyNotFoundException($"User with ID {UserId} not found.");
            }
            return user;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {email} not found.");
            }
            return user;
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = _userRepository.GetAllUsersAsync();
            if(users == null)
            {
                throw new KeyNotFoundException("No users found.");
            }
            return users;
        }

        public Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            var users = _userRepository.GetUsersByRoleAsync(role);
            if (users == null)
            {
                throw new KeyNotFoundException($"No users found for {role}.");
            }
            return users;
        }


        public async Task AddUserAsync(User user)
        {
            User userExists = await _userRepository.GetUserByEmailAsync(user.Email);
            if (userExists != null)
            {
                throw new KeyNotFoundException($"User with this {user.Email} already exists.");
            }
            if (user.DateOfBirth > DateTime.Now.AddYears(-18))
            {
                throw new KeyNotFoundException("User must be at least 18 years old.");
            }
            if (string.IsNullOrEmpty(user.PasswordHash) || user.PasswordHash.Length < 8)
            {
                throw new KeyNotFoundException("Password must be at least 8 characters long.");
            }
            // Validate email format and mark as verified only if format is valid
            var emailValidator = new EmailAddressAttribute();
            user.IsEmailVerified = emailValidator.IsValid(user.Email);
            if(!user.IsEmailVerified)
            {
                throw new KeyNotFoundException("Invalid email format.");
            }
            user.CreatedAt = DateTime.Now;
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            User userExists = await _userRepository.GetUserByIdAsync(user.UserId);
            if (userExists == null)
            {
                throw new KeyNotFoundException($"User with ID {user.UserId} not found.");
            }
            user.UpdatedAt = DateTime.Now;
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            User userExists = await _userRepository.GetUserByIdAsync(userId);
            if (userExists == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            await _userRepository.DeleteUserAsync(userId);
        }

       
    }
}
