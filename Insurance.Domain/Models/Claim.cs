using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models // <--- ADD THIS LINE
{
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
        public CustomerPolicy customerPolicy { get; set; }
        public Claim()
        {
        }
    }
}