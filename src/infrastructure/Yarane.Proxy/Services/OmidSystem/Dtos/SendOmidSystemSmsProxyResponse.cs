namespace Yarane.Proxy.Services.OmidSystem.Dtos
{

    public class SendOmidSystemSmsProxyResponse
    {
        public string Message { get; set; }
        public bool Succeeded { get; set; }
        public List<string> Data { get; set; }
        public int ResultCode { get; set; }

    }


}
