using Newtonsoft.Json;

namespace Yarane.Common.ReturnResults
{
    public class ProxyResult
    {
        public bool IsSuccess { get; set; }
        public int RsCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
        public ProxyResult(bool isSuccess, int rsCode, string message)
        {
            IsSuccess = isSuccess;
            RsCode = rsCode;
            Message = message;
        }
    }
    public class ProxyResult<TResultData> : ProxyResult
        where TResultData : class
    {
        public TResultData Data { get; set; }
        public ProxyResult(bool isSuccess, int rsCode, TResultData resultData, string message = null)
            : base(isSuccess, rsCode, message)
        {
            Data = resultData;
        }
        #region implicit operators
        public static implicit operator ProxyResult<TResultData>(TResultData resultData)
        {
            return new ProxyResult<TResultData>(true, 0, resultData);
        }
        #endregion
    }
}
