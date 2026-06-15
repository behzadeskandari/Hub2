namespace Yarane.Proxy.Services.OmidSystem.Dtos
{
    public class SendOmidSystemProxyRequest
    {
        public List<SendOmidSystemProxyData> Messages { get; set; }
    }

    public class SendOmidSystemProxyData
    {
        public List<string> Destinations { get; set; }
        public List<string> Parameters { get; set; }
        public string PatternId { get; set; }
    }

}





