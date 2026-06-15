namespace Yarane.Proxy.Services.Dotin.Dtos
{
    public class SendSmsProxyRequest
    {
        public string? MessageBody { get; set; }
        public string? MobileNumbers { get; set; }
        public string? SmsType { get; set; }
        public string? Scheduler { get; set; }
        public string? TerminalType { get; set; }
    }

}
