using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yarane.Contract.Dtos.Report
{
    public class ReportDto
    {
        public string Source { get; set; }
        public string TrackingNumber { get; set; }
        public string Rrn { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string ApprovalCode { get; set; }
    }
}
