using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insurance.Application.DTOs.CustomerPolicyDTO
{
    public class UpdateCustomerPolicyStatusDto
    {
        [Required]
        public int CustomerPolicyId { get; set; }

        [Required]
        public int Status { get; set; } // Map to your CustomerPolicyStatus enum integer
    }
}
