using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insurance.Application.DTOs.CustomerPolicyDTO
{
    public class PurchasePolicyDto
    {
        [Required]
        public int UserId { get; set; } // Who is buying it

        [Required]
        public int PolicyId { get; set; } // Which plan they are buying

        public int? AgentId { get; set; } // Optional: null if bought online directly, contains ID if sold by an agent
    }
}
