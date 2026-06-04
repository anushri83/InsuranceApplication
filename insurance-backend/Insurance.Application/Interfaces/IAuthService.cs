using Insurance.Application.DTOs.UserDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Interfaces
{
    public interface IAuthService
    {
        // Validates credentials and returns a JWT token string if successful
        Task<string> LoginAsync(LoginDto dto);
    }
}
