using Newtonsoft.Json;

namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    //public class OutputListProxyResponse
    //{
    //    public string Message { get; set; }
    //    public List<OutputListProxyResultData> Data { get; set; }
    //}

    //public class OutputListProxyResultData
    //{
    //    [JsonProperty("requestDate")]
    //    public string RequestDate { get; set; }

    //    [JsonProperty("identifier")]
    //    public string Identifier { get; set; }

    //    [JsonProperty("totalApplicantCount")]
    //    public int TotalApplicantCount { get; set; }

    //    [JsonProperty("hasAccountApplicantCount")]
    //    public int HasAccountApplicantCount { get; set; }
    //}


    public class OutputListProxyResponse
    {
        public ResultDto Result { get; set; }
        public Status Status { get; set; }
    }

    public class ResultDto
    {
        public DataWrapperDto Data { get; set; }
        public Status Status { get; set; }
    }

    public class DataWrapperDto
    {
        public List<Data> Data { get; set; }
        public string Message { get; set; }
    }

    public class Data
    {
        public string RequestDate { get; set; }
        public string Identifier { get; set; }
        public int TotalApplicantCount { get; set; }
        public int HasAccountApplicantCount { get; set; }
        public int WithoutAccountApplicantCount { get; set; }
        public int ErrorApplicantCount { get; set; }
    }

    public class Status
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
