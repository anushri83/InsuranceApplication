using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models;

public class Policy
{
    [Key]
    public int PolicyId { get; set; }

    [Required]
    [MaxLength(150)]
    public string PolicyName { get; set; } = string.Empty; // Initialize to avoid warning

    public string? Description { get; set; } // Nullable if description is optional

    [Required]
    public decimal PremiumAmount { get; set; }

    [Required]
    public int DurationInMonth { get; set; }

    public Policy() { }
}