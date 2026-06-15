namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class BankTransactionSubmitProxyResponse
    {
        public List<BankTransactionSubmitProxyResultData> ResultData { get; set; }
    }

    public class BankTransactionSubmitProxyResultData
    {
        public string Message { get; set; }
    }
}
