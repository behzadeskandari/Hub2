namespace Yarane.Contract.Dtos
{
    public class SendSmsResponseDto
    {
        public string? NationalId { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsSuccess { get; set; }
        public bool? RsCode { get; set; }
    }
}
