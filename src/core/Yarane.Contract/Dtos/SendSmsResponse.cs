using Newtonsoft.Json;

namespace Yarane.Contract.Dtos
{
    public class SendSmsResponse
    {
        //public SendSmsRequestData RequestDatas { get; set; }
    }

    public class SendSmsRequestData
    {
        public Return Return { get; set; }
        public List<Entry> Entries { get; set; }
    }

    public class Return
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }

    public class Entry
    {
        public long MessageId { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public string Sender { get; set; }
        public string Receptor { get; set; }
        public long Date { get; set; }
        public int Cost { get; set; }
    }

}
