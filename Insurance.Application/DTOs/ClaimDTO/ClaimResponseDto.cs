using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insurance.Application.DTOs.ClaimDTO
{
    public class ClaimResponseDto
    {
        public int ClaimId { get; set; }
        public int CustomerPolicyId { get; set; }
        public string PolicyName { get; set; } = string.Empty;

        // 💡 Pulls data from deep inside the relational database chain:
        // Claim -> CustomerPolicy -> User
        public string CustomerName { get; set; } = string.Empty;

        public decimal ClaimAmount { get; set; }
        public string Status { get; set; } = string.Empty; // "Pending", "Approved", "Rejected"
        public DateTime CreatedAt { get; set; }
    }
}
