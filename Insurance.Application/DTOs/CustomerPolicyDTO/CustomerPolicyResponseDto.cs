using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.DTOs.CustomerPolicyDTO
{
    public class CustomerPolicyResponseDto
    {
        public int CustomerPolicyId { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public int PolicyId { get; set; }
        public string PolicyName { get; set; } = string.Empty;
        public decimal PremiumAmount { get; set; }

        public string? AgentName { get; set; } // "Direct Online" if AgentId is null

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty; // e.g., "Active", "Terminated"
    }
}
