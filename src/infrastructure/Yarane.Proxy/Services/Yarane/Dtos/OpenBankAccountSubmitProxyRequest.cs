namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class OpenBankAccountSubmitProxyRequest
    {
        public List<OpenBankAccountSubmitProxyRequestData> RequestDatas { get; set; }
    }

    public class OpenBankAccountSubmitProxyRequestData
    {
        public string NationalCode { get; set; }
        public string AccountNumber { get; set; }
        public string CardNumber { get; set; }
        public string ShebaNumber { get; set; }
        public string BranchCode { get; set; }
        public string OpeningDate { get; set; }
        public string ShahabCode { get; set; }
        public bool IsRevoked { get; set; }

    }
}
