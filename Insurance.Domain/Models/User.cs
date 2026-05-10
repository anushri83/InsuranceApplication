using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }

    [Required, Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public Gender Gender { get; set; }

    public string? AddressLine1 { get; set; }

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    [StringLength(6)]
    public string Pincode { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;

    [Required]
    [StringLength(12)]
    public string AadhaarNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string PANNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public bool IsEmailVerified { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}