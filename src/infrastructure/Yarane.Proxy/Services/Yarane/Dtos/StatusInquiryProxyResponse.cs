namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class StatusInquiryProxyResponse
    {
        public string Message { get; set; }
        public List<StatusInquiryProxyResultData> Data { get; set; }
    }
    public class StatusInquiryProxyResultData
    {
        public string NationalCode { get; set; }
        public string ApplicantStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastStatusChangeDate { get; set; }
        public string BirthCertificateNumber { get; set; }
        public string BirthCertificateSerial { get; set; }
        public string BirthCertificateCharacter { get; set; }
        public string BirthCertificateVersion { get; set; }

    }
}


