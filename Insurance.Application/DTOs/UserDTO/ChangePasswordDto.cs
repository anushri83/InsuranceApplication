using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insurance.Application.DTOs.UserDTO
{
    public class ChangePasswordDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string OldPassword { get; set; } = string.Empty;

        [Required, MinLength(8, ErrorMessage = "New password must be at least 8 characters long.")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
