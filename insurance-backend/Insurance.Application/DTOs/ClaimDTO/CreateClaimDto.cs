using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insurance.Application.DTOs.ClaimDTO
{
    public class CreateClaimDto
    {
        [Required]
        public int CustomerPolicyId { get; set; } // Which active policy are they claiming against?

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Claim amount must be greater than zero.")]
        public decimal ClaimAmount { get; set; } // How much money are they requesting?
    }
}
