using Newtonsoft.Json;

namespace Yarane.Contract.Dtos
{
    public class SendOmidSystemSmsResponse
    {
        public string Message { get; set; }
        public bool Succeeded { get; set; }
        public List<string> Data { get; set; }
        public int ResultCode { get; set; }

    }
}
