using Newtonsoft.Json;

namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class OpenBankAccountSubmitProxyResponse
    {
        public string Message { get; set; }

        [JsonProperty("data")]
        public List<OpenBankAccountSubmitProxyResultData> ResultData { get; set; }
    }

    public class OpenBankAccountSubmitProxyResultData
    {
        [JsonProperty("nationalCode")]
        public string NationalCode { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

    }

}
