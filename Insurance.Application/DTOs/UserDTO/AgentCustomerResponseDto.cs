using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.DTOs.UserDTO
{
    public class AgentCustomerResponseDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? City { get; set; }
        public bool IsActive { get; set; }

       
    }
}
