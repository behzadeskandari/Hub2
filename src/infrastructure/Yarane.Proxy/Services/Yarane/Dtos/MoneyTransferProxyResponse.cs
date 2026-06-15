namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class MoneyTransferProxyResponse
    {
        public List<MoneyTransferProxyResultData>? Data { get; set; } = new();
        public string? Message { get; set; }
    }
    public class MoneyTransferProxyResultData
    {
        public string? DestAccountNumber { get; set; }
        public string? RefNumber { get; set; }
        public string? Identifier { get; set; }
        public string? PaymentIdentifier { get; set; }
        public string? NationalCode { get; set; }
        public string? SourceAccountNumber { get; set; }
        public string? Amount { get; set; }
    }
}
