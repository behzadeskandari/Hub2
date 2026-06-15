namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class OutputGenerateProxyResponse
    {
        public string Message { get; set; }
        public OutputGenerateProxyResultData ResultData { get; set; }
    }

    public class OutputGenerateProxyResultData
    {
        public string Identifier { get; set; }
        public List<OutputGenerateProxyItem> Items { get; set; }
    }


    public class OutputGenerateProxyItem
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
