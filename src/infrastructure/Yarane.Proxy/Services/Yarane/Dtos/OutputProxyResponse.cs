namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class OutputProxyResponse
    {
        public string Message { get; set; }
        public List<OutputProxyResultData> Data { get; set; }
    }
    public class OutputProxyResultData
    {
        public string ApplicantId { get; set; }
        public string NationalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string BirthCertificateNumber { get; set; }
        public string BirthCertificateSerial { get; set; }
        public string BirthCertificateCharacter { get; set; }
        public string FatherName { get; set; }
        public string MarriageStatus { get; set; }
        public string Gender { get; set; }
        public string ResidentPostCode { get; set; }
        public string ResidentAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}
