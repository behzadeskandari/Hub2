namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class AccountBlockSubmitProxyRequest
    {
        public List<AccountBlockSubmitProxyRequestData> RequestDatas { get; set; }
    }

    public class AccountBlockSubmitProxyRequestData
    {
        public string NationalCode { get; set; }
        public string AccountNo { get; set; }
        public string BlockAmount { get; set; }
        public bool IsCompletelyBlocked { get; set; }
    }
}
