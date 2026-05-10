using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models;
public class Claim
{
    [Key]
    public int ClaimId { get; set; }

    [ForeignKey("CustomerPolicy")]
    public int CustomerPolicyId { get; set; }

    [Required]
    public decimal ClaimAmount { get; set; }

    [Required]
    public ClaimStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Use ? here because this is a navigation property
    public CustomerPolicy? customerPolicy { get; set; }

    public Claim() { }
}