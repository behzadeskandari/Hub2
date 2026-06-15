namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class ProjectPaymentProxyResponse
    {
        public List<ProjectPaymentProxyResultData>? Data { get; set; } = new();
        public string? Message { get; set; }
    }
    public class ProjectPaymentProxyResultData
    {
        public string? Amount { get; set; }
        public string? ApplicantAccountNumber { get; set; }
        public string? NationalCode { get; set; }
        public string? PaymentIdentifier { get; set; }
        public string? DestAccountNumber { get; set; }
        public string? RefNumber { get; set; }
        public string? ProjectPaymentId { get; set; }
    }
}
