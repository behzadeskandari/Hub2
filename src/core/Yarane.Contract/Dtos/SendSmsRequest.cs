using Newtonsoft.Json;

namespace Yarane.Contract.Dtos
{
    public class SendSmsRequest
    {
        public string? MessageBody { get; set; }
        public List<string>? MobileNumbers { get; set; }
        public string? SmsType { get; set; }
        public string? Scheduler { get; set; }
        public string? TerminalType { get; set; }
    }

}
