namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class OpenBankAccountErrorSubmitProxyResponse
    {
        public string Message { get; set; }
        public List<OpenBankAccountErrorSubmitProxyResultData> ResultData { get; set; }
    }

    public class OpenBankAccountErrorSubmitProxyResultData
    {
        //toDo When I wanted to complete this class, the connection with the urban planning service was lost
    }
}
