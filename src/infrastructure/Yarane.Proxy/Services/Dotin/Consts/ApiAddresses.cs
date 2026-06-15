namespace Yarane.Proxy.Services.Dotin.Consts;
public class ApiAddresses
{
    public static string GetStatementByAccountNumber = "/Api/GetStatementByAccountNumber";
    public static string GetDepositBalance = "/Api/GetDepositBalance";
    public static string InquiryOtherBankIbanInfo = "/Api/InquiryOtherBankIbanInfo";
    public static string TransferSatna = "/TransferPublic/TransferSatna";
    public static string TransferPaya = "/TransferPublic/TransferPaya";
    public static string InternalTransfer = "/Api/InternalTransfer";
    public static string GetTransactionStatus = "/Api/Inquiry/GetTransactionStatus";
    public static string SearchPayaDetails = "/Api/SearchPayaDetails";
    public static string SearchSatnaDetails = "/Api/SearchSatnaDetails";
    public static string GetAccountInfo = "/Api/GetAccountInfo";
    public static string SearchBlockAndUnblockLetter = "/ESB/SearchBlockAndUnblockLetter";
    public static string GetLoanList = "/Api/GetLoanList";
    public static string SmsLink = "/Wrapper/SendSmsByQueue";

}
