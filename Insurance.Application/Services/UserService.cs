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

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            if(users == null)
            {
                throw new KeyNotFoundException("No users found.");
            }
            return users;
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            var users = await _userRepository.GetUsersByRoleAsync(role);
            if (users == null)
            {
                throw new KeyNotFoundException($"No users found for {role}.");
            }
            return users;
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

        public async Task RegisterCustomerAsync(CreateCustomerDto dto)
        {
            //  Map DTO fields to a real User Entity
            var user = new User
            {
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

                //  Set backend security rules automatically
                Role = UserRole.Customer, // Hardcoded protection!
                PasswordHash = HashPassword(dto.Password), // Turn plain text into hash
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await ValidateNewUserRulesAsync(user, dto.Password);
            await _userRepository.RegisterUserAsync(user);
        }

        public async Task RegisterAgentAsync(CreateAgentDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                AddressLine1 = dto.AddressLine1,
                City = dto.City,
                State = dto.State,
                Pincode = dto.Pincode,

                Role = UserRole.Agent, // Safe automation
                PasswordHash = HashPassword(dto.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await ValidateNewUserRulesAsync(user, dto.Password);
            await _userRepository.RegisterUserAsync(user);
        }

        public async Task RegisterAdminAsync(CreateAdminDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,

                Role = UserRole.Admin, // Safe automation
                PasswordHash = HashPassword(dto.Password),
                IsActive = true,
                IsEmailVerified = true, // Admins can be pre-verified
                CreatedAt = DateTime.UtcNow
            };

            await ValidateNewUserRulesAsync(user, dto.Password);
            await _userRepository.RegisterUserAsync(user);
        }

       
        

        public async Task UpdateUserAsync(UpdateUserDto dto)
        {
            User existingUser = await _userRepository.GetUserByIdAsync(dto.UserId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {dto.UserId} not found.");
            }
            existingUser.Name = dto.Name;
            existingUser.PhoneNumber = dto.PhoneNumber;
            existingUser.DateOfBirth = dto.DateOfBirth;
            existingUser.Gender = dto.Gender.HasValue ? (Gender)dto.Gender.Value : null;
            existingUser.AddressLine1 = dto.AddressLine1;
            existingUser.City = dto.City;
            existingUser.State = dto.State;
            existingUser.Pincode = dto.Pincode;
            existingUser.UpdatedAt = DateTime.Now;
            await _userRepository.UpdateUserAsync(existingUser);
        }

        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            User existingUser= await _userRepository.GetUserByIdAsync(dto.UserId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {dto.UserId} not found.");
            }
            bool isOldPasswordCorrect = VerifyPassword(dto.OldPassword, existingUser.PasswordHash);
            if (!isOldPasswordCorrect)
            {
                throw new InvalidOperationException("The old password you entered is incorrect.");
            }
            if (dto.OldPassword == dto.NewPassword)
            {
                throw new InvalidOperationException("New password cannot be the same as your old password.");
            }
            // Securely hash the new password and apply it
            existingUser.PasswordHash = HashPassword(dto.NewPassword);
            existingUser.UpdatedAt = DateTime.Now;

            await _userRepository.UpdateUserAsync(existingUser);
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
