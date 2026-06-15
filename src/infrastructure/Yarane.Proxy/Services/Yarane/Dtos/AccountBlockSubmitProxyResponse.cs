namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class AccountBlockSubmitProxyResponse
    {
        public string Message { get; set; }
        public List<AccountBlockSubmitProxyResultData> ResultData { get; set; }
    }

    public class AccountBlockSubmitProxyResultData
    {
        //toDo When I wanted to complete this class, the connection with the urban planning service was lost
        public string Message { get; set; }
    }
}
