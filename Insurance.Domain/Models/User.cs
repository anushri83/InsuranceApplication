using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }

    [Required, Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    // Optional for Admins/Agents, Required for Customers (Handled in Service)
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string? AadhaarNumber { get; set; }
    public string? PANNumber { get; set; }

    public string? AddressLine1 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Pincode { get; set; }

    // Add this back because your UserService needs it
    public bool IsEmailVerified { get; set; } = false;

    // Ensure this exists too, as it's common for status checks
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}