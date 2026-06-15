using Newtonsoft.Json;

namespace Yarane.Contract.Dtos
{
    public class SendSmsSingleRequest
    {
        public string? MessageBody { get; set; }
        public string? MobileNumbers { get; set; }
        public string? SmsType { get; set; }
        public string? Scheduler { get; set; }
        public string? TerminalType { get; set; }
    }

}
