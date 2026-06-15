using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yarane.Domain.Entities
{
    public class TransferCardTransAction : BaseEntity
    {
        public string TrackingNumber { get; set; } = null!;

        public string? SourcePan { get; set; }
        public string? DestinationPan { get; set; }

        public string? TerminalNumber { get; set; }
        public string? AcceptorCode { get; set; }

        public decimal? Amount { get; set; }   // NUMBER(19)

        public string? ReferenceNumber { get; set; }

        public int? TerminalType { get; set; }
        public int? Localization { get; set; }

        public long? RegistrationDate { get; set; }
        public long? TransactionDate { get; set; }

        public long? Stan { get; set; }

        public string? RRN { get; set; }

        public string? AdditionalResponseData { get; set; }

        public string? ApprovalCode { get; set; }

        public int? Status { get; set; }

        public string? ErrorJson { get; set; }

        public string? SourceAddress { get; set; }

        public int? TransactionType { get; set; }

        public string? MerchantID { get; set; }
    }
}
