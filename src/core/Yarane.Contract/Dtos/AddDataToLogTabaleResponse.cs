namespace Yarane.Contract.Dtos
{

    public class AddDataToLogTabaleResponse
    {
        public AddDataToLogTabaleResultData ResultData { get; set; }
    }

    public class AddDataToLogTabaleResultData
    {
        public string MessageForFaild { get; set; }
        public string MessageForSuccess { get; set; }
    }

}
