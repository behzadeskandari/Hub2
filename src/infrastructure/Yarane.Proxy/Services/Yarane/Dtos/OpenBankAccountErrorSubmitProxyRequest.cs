namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class OpenBankAccountErrorSubmitProxyRequest
    {
        public List<OpenBankAccountErrorSubmitProxyRequestData> RequestDatas { get; set; }
    }

    public class OpenBankAccountErrorSubmitProxyRequestData
    {
        public string NationalCode { get; set; }
        public string Error { get; set; }

    }
}
