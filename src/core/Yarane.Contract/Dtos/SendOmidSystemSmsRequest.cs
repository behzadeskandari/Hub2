using Newtonsoft.Json;

namespace Yarane.Contract.Dtos
{
    public class SendOmidSystemSmsRequest
    {
        // public List<MessageRequestData> Messages { get; set; }
        public List<string> Destinations { get; set; }
        public List<string> Parameters { get; set; }
        public string PatternId { get; set; }

    }

    //public class MessageRequestData
    //{

    //}

}
