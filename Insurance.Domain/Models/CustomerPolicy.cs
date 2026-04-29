using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models // <--- ADD THIS LINE
{
    public class CustomerPolicy
    {
        [Key]
        public int CustomerPolicyId { get; set; }
        [ForeignKey("User")]  //connect table with user table
        public int UserId { get; set; }
        [ForeignKey("Policy")] //connect table with policy table
        public int PolicyId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public PolicyStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User User { get; set; } // Navigation property to User
        public Policy Policy { get; set; }  // Navigation property to Policy
        public CustomerPolicy()
        {
        }
    }
}