namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class ProjectContractSubmitProxyResponse
    {
        public string Message { get; set; }
        public List<ProjectContractSubmitProxyResultData> Data { get; set; }
    }

    public class ProjectContractSubmitProxyResultData
    {
        //toDo When I wanted to complete this class, the connection with the urban planning service was lost
        public bool IsSuccess { get; set; }
        public string ContractNumber { get; set; }
        public string Message { get; set; }
    }
}
