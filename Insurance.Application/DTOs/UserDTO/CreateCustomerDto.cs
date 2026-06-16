using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insurance.Application.DTOs.UserDTO
{
    public class CreateCustomerDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int Gender { get; set; } // Passed as integer matching your Gender enum

        [Required]
        public string AadhaarNumber { get; set; } = string.Empty;

        [Required]
        public string PANNumber { get; set; } = string.Empty;
        [Required]
        public string AddressLine1 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Pincode { get; set; }

        [Required]
        public string BankName { get; set; } = string.Empty;

        [Required]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        public string IFSCCode { get; set; } = string.Empty;
    }
}

