using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Application.DTOs.BusinessTopics
{
    public class CreateBusinessTopicDto
    {
        public int TenantId { get; set; }

        public int AgreementId { get; set; }

        public int BusinessPartnerId { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? ReferenceNumber { get; set; }

        public DateTime TopicDate { get; set; }

        public string? RawPayload { get; set; }
    }
}
