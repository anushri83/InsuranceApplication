using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insurance.Application.DTOs.UserDTO
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } =string.Empty; // Initialize to empty string to avoid null reference issues
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
