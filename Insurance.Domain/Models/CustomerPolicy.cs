using Insurance.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CustomerPolicy
{
    [Key]
    public int CustomerPolicyId { get; set; }

    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; } // The Customer

    [Required]
    [ForeignKey("Policy")]
    public int PolicyId { get; set; } // The Insurance Product

    // --- NEW FIELD ---
    [ForeignKey("Agent")]
    public int? AgentId { get; set; } // The Agent who sold the policy (can be null if bought directly)

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public CustomerPolicyStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    public User? User { get; set; } // The Customer object
    public Policy? Policy { get; set; }
    public User? Agent { get; set; } // The Agent object (points to User table)
}