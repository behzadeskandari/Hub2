using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yarane.Domain.Entities
{
    public class CardHolderTransaction : BaseEntity
    {
        public string? TrackingNumber { get; set; }
        public string? SourcePan { get; set; }
        public string? DestinationPan { get; set; }
        public string? TerminalNumber { get; set; }
        public string? AcceptorCode { get; set; }

        public decimal? Amount { get; set; }

        public string? ReferenceNumber { get; set; }

        public int? TerminalType { get; set; }
        public int? Localization { get; set; }

        public string? SourceAddress { get; set; }

        public long? RegistrationDate { get; set; }
        public long? TransactionDate { get; set; }

        public string? RRN { get; set; }

        public string? AdditionalResponseData { get; set; }

        public string? CardHolderName { get; set; }

        public string? ApprovalCode { get; set; }

        public int Status { get; set; }

        public string? TwoPhaseReferenceCode { get; set; }
    }
}
