using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yarane.Contract.Dtos.Transaction
{
    public class TransactionReportVm
    {
        public string Source { get; set; }

        public decimal? Amount { get; set; }
        public int? Status { get; set; }
        public long? TransactionDate { get; set; }

        public string TrackingNumber { get; set; }
        public string RRN { get; set; }
        public string ApprovalCode { get; set; }

        public string? SourcePan { get; set; }
        public string? DestinationPan { get; set; }
        public string? TerminalNumber { get; set; }
        public string? AcceptorCode { get; set; }

        public string? ReferenceNumber { get; set; }
        public int? TerminalType { get; set; }
        public int? Localization { get; set; }
        public long? RegistrationDate { get; set; }

        public string? SourceAddress { get; set; }
        public string? AdditionalResponseData { get; set; }

        public string? CardHolderName { get; set; }
        public string? TwoPhaseReferenceCode { get; set; }

        public string? Error_Json { get; set; }
        public decimal? TransferAmount { get; set; }
        public int? TransferStatus { get; set; }
        public string? TransferErrorJson { get; set; }
        public int TransferTerminalType { get; set; }
        public string? TransferHistErrorJson { get; set; }
        public string? ConsumerName { get; set; }
    }


    public class TransactionSearchRequest
    {
        // 🔍 Common filters
        public string? TrackingNumber { get; set; }
        public string? RRN { get; set; }
        public int? Status { get; set; }
        public decimal? AmountFrom { get; set; }
        public decimal? AmountTo { get; set; }

        // 📅 Date filtering (recommended as DateTime, not long ticks)
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        // 🏷 Optional filters (useful for TransferCardTransAction)
        public string? TransactionType { get; set; }
        public string? MerchantId { get; set; }

        // 📦 Pagination (used only for Search endpoint, ignored in ExportAll)
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;

        // 🔽 Sorting (optional but useful for AG Grid)
        public string? SortField { get; set; } = "TransactionDate";
        public string? SortDirection { get; set; } = "desc";
    }
}
