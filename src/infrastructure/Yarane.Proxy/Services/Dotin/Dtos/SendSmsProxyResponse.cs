namespace Yarane.Proxy.Services.Dotin.Dtos
{
    //public class SendSmsProxyResponse
    //{
    //    public SendSmsProxyRequestData RequestDatas { get; set; }
    //}

    public class SendSmsProxyResponse
    {
        public bool IsSuccess { get; set; }
        public int RsCode { get; set; }
        public string Message { get; set; }
    }


}
