using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models;
public class CustomerPolicy
{
    [Key]
    public int CustomerPolicyId { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Policy")]
    public int PolicyId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public PolicyStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation properties marked as nullable
    public User? User { get; set; }
    public Policy? Policy { get; set; }

    public CustomerPolicy() { }
}