using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models // <--- ADD THIS LINE
{
    public class Policy
    {
        [Key]
        public int PolicyId { get; set; }
        [Required]
        [MaxLength(150)]
        public string PolicyName { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal PremiumAmount { get; set; }
        [Required]
        public int DurationInMonth { get; set; }
        public Policy()
        {
        }
    }
}
