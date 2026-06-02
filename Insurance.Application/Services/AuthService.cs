using Insurance.Application.DTOs.UserDTO;
using Insurance.Application.Interfaces;
using Insurance.Domain.Interfaces;
using Insurance.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

//AuthService handles the complete login authentication process. It checks whether the user exists, verifies the password using BCrypt hashing
//and use the exact configuration keys from your appsettings.json, and if authentication is successful,
//generates a secure JWT token containing the user’s identity and role information.

namespace Insurance.Application.Services
{
    public  class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        // The IConfiguration interface allows you to access configuration settings from appsettings.json or environment variables,
        
        private readonly IConfiguration _configuration;
        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            try
            {
                var userexist = await _userRepository.GetUserByEmailAsync(dto.Email);
                if (userexist == null)
                {
                    return null; // User not found
                }
                
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, userexist.PasswordHash))
                { 
                    return null; // Password does not match
                }

                return GenerateJwtToken(userexist.UserId, userexist.Email, userexist.Role);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the login request.", ex);
            }
        }

        private string GenerateJwtToken(int userId, string email, UserRole role)
        {
            try
            {
                var JWTSetting = _configuration.GetSection("JWT");
                var secretKey = JWTSetting["SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key is missing.");

                // Claims = user information stored inside token (userId, email, role) that send to frontend with header so can be decode safely
                // and help identify the logged-in user
                var claims = new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new System.Security.Claims.Claim(ClaimTypes.Email, email),
                    new System.Security.Claims.Claim(ClaimTypes.Role, role.ToString())
                };

                //  Convert our text SecretKey into a encrypted byte array that required for token signature
                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                //Actaully digitaly sign the token using the encrypted key and specify the hashing algorithm (HMAC SHA256) to ensure the token's integrity and authenticity
                var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

                // JwtSecurityToken is a class that represents the structure of a JWT token
                var token = new JwtSecurityToken(
                    issuer: JWTSetting["Issuer"],
                    audience: JWTSetting["Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(int.Parse(JWTSetting["DurationInMinutes"] ?? "60")),
                    signingCredentials: creds
                );

                // Convert the Token object into a single encrypted string that we can send to the client
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
