namespace Yarane.Proxy.Services.NehzatMelli.Dtos
{
    public class ProjectContractSubmitProxyRequest
    {
        public List<ProjectContractSubmitProxyRequestData> RequestDatas { get; set; }
    }

    public class ProjectContractSubmitProxyRequestData
    {
        public string ContractNumber { get; set; }
        public string ContractDate { get; set; }
        public string ContractEndDate { get; set; }
        public long ContractPrice { get; set; }
        public int PaidLoanCount { get; set; }
        public long PaidLoanAmount { get; set; }
        public int ProjectProcessPercentage { get; set; }
        public string LastUpdateDate { get; set; }
        public int UnitCount { get; set; }
        public string VisitDate { get; set; }
        public bool IsRevoked { get; set; }
        public decimal InterestRate { get; set; }
    }
}
