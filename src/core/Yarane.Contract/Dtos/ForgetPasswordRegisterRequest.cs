namespace Yarane.Contract.Dtos
{
    public class ForgetPasswordRegisterRequest
    {
        public long? Phone { get; set; }
        public string? UserName { get; set; }
        public string Key { get; set; }
        public string Captcha { get; set; }

    }

}
