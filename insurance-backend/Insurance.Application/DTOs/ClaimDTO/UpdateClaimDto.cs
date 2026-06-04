using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insurance.Application.DTOs.ClaimDTO
{
    public class UpdateClaimDto
    {
        [Required]
        public int ClaimId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Claim amount must be greater than zero.")]
        public decimal ClaimAmount { get; set; } // 💡 Added this so it can be updated!

        [Required]
        public int Status { get; set; } // Maps to your ClaimStatus Enum
    }
}
