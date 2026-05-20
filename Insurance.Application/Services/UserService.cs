using Insurance.Application.DTOs.UserDTO;
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

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);

                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {userId} not found.");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching user by ID.", ex);
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);

                if (user == null)
                {
                    throw new KeyNotFoundException($"User with email {email} not found.");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching user by email.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();

                if (users == null || !users.Any())
                {
                    throw new KeyNotFoundException("No users found.");
                }

                return users;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching users.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            try
            {
                var users = await _userRepository.GetUsersByRoleAsync(role);

                if (users == null || !users.Any())
                {
                    throw new KeyNotFoundException($"No users found for role {role}.");
                }

                return users;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching users by role.", ex);
            }
        }

        public async Task RegisterCustomerAsync(CreateCustomerDto dto)
        {
            try
            {
                var user = new User
                {
                    UserId = 0, 
                    Name = dto.Name,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = (Gender)dto.Gender,
                    AadhaarNumber = dto.AadhaarNumber,
                    PANNumber = dto.PANNumber,
                    AddressLine1 = dto.AddressLine1,
                    City = dto.City,
                    State = dto.State,
                    Pincode = dto.Pincode,

                    Role = UserRole.Customer,
                    PasswordHash = HashPassword(dto.Password),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await ValidateNewUserRulesAsync(user, dto.Password);

                await _userRepository.RegisterUserAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while registering customer.", ex);
            }
        }

        public async Task RegisterAgentAsync(CreateAgentDto dto)
        {
            try
            {
                var user = new User
                {
                    UserId = 0,
                    Name = dto.Name,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    AddressLine1 = dto.AddressLine1,
                    City = dto.City,
                    State = dto.State,
                    Pincode = dto.Pincode,

                    Role = UserRole.Agent,
                    PasswordHash = HashPassword(dto.Password),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await ValidateNewUserRulesAsync(user, dto.Password);

                await _userRepository.RegisterUserAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while registering agent.", ex);
            }
        }

        public async Task RegisterAdminAsync(CreateAdminDto dto)
        {
            try
            {
                var user = new User
                {
                    UserId = 0,
                    Name = dto.Name,
                    Email = dto.Email,

                    Role = UserRole.Admin,
                    PasswordHash = HashPassword(dto.Password),
                    IsActive = true,
                    IsEmailVerified = true,
                    CreatedAt = DateTime.UtcNow
                };

                await ValidateNewUserRulesAsync(user, dto.Password);

                await _userRepository.RegisterUserAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while registering admin.", ex);
            }
        }

        public async Task UpdateUserAsync(UpdateUserDto dto)
        {
            try
            {
                User existingUser = await _userRepository.GetUserByIdAsync(dto.UserId);

                if (existingUser == null)
                {
                    throw new KeyNotFoundException($"User with ID {dto.UserId} not found.");
                }

                existingUser.Name = dto.Name;
                existingUser.PhoneNumber = dto.PhoneNumber;
                existingUser.DateOfBirth = dto.DateOfBirth;
                existingUser.Gender = dto.Gender.HasValue
                    ? (Gender)dto.Gender.Value
                    : null;
                existingUser.AddressLine1 = dto.AddressLine1;
                existingUser.City = dto.City;
                existingUser.State = dto.State;
                existingUser.Pincode = dto.Pincode;
                existingUser.UpdatedAt = DateTime.Now;

                await _userRepository.UpdateUserAsync(existingUser);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating user.", ex);
            }
        }

        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            try
            {
                User existingUser = await _userRepository.GetUserByIdAsync(dto.UserId);

                if (existingUser == null)
                {
                    throw new KeyNotFoundException($"User with ID {dto.UserId} not found.");
                }

                bool isOldPasswordCorrect =
                    VerifyPassword(dto.OldPassword, existingUser.PasswordHash);

                if (!isOldPasswordCorrect)
                {
                    throw new InvalidOperationException("Old password is incorrect.");
                }

                if (dto.OldPassword == dto.NewPassword)
                {
                    throw new InvalidOperationException("New password cannot be same as old password.");
                }

                existingUser.PasswordHash = HashPassword(dto.NewPassword);
                existingUser.UpdatedAt = DateTime.Now;

                await _userRepository.UpdateUserAsync(existingUser);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while changing password.", ex);
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            try
            {
                User userExists = await _userRepository.GetUserByIdAsync(userId);

                if (userExists == null)
                {
                    throw new KeyNotFoundException($"User with ID {userId} not found.");
                }

                await _userRepository.DeleteUserAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting user.", ex);
            }
        }


        // Simple placeholder helper for password hashing logic
        private string HashPassword(string password)=>
             Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        

        private bool VerifyPassword(string inputPassword, string storedHash) =>
            HashPassword(inputPassword) == storedHash;

        private async Task ValidateNewUserRulesAsync(User user, string plainTextPassword)
        {
            //  Check if email is already taken
            User userExists = await _userRepository.GetUserByEmailAsync(user.Email);
            if (userExists != null)
            {
                throw new InvalidOperationException($"User with email {user.Email} already exists.");
            }

            // Age check (Only apply to Customers, since Admins/Agents might not have DoB populated yet)
            if (user.Role == UserRole.Customer && user.DateOfBirth > DateTime.Now.AddYears(-18))
            {
                throw new InvalidOperationException("User must be at least 18 years old.");
            }

            //  Password length check (Check the raw password string before hashing!)
            if (string.IsNullOrEmpty(plainTextPassword) || plainTextPassword.Length < 8)
            {
                throw new InvalidOperationException("Password must be at least 8 characters long.");
            }

            // Validate email format
            var emailValidator = new EmailAddressAttribute();
            user.IsEmailVerified = emailValidator.IsValid(user.Email);
            if (!user.IsEmailVerified)
            {
                throw new InvalidOperationException("Invalid email format.");
            }
        }

        

       
    }
}
