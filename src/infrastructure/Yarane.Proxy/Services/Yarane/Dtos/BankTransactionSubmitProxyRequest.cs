namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class BankTransactionSubmitProxyRequest
    {
        public List<BankTransactionSubmitProxyRequestData> RequestDatas { get; set; }
    }

    public class BankTransactionSubmitProxyRequestData
    {
        public string NationalCode { get; set; }
        public string AccountNumber { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string CreditAmount { get; set; }
        public string DebitAmount { get; set; }
        public string DestAccountNumber { get; set; }
        public string BranchCode { get; set; }

    }
}
