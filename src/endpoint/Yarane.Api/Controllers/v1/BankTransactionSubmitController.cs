using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NehzatMelli.Contract.Dtos;
using NehzatMelli.Contract.Interfaces.ApplicationServices;

namespace NehzatMelli.Api.Controllers.v1
{

    public class BankTransactionSubmitController : BaseController
    {
        #region constractor
        private readonly IBankTransactionSubmitService _bankTransactionSubmitService;
        public BankTransactionSubmitController(IBankTransactionSubmitService bankTransactionSubmitService)
        {
            this._bankTransactionSubmitService = bankTransactionSubmitService;
        }
        #endregion


        [HttpGet]
        public async Task<IActionResult> BankTransactionSubmit()
        {
            var requestDatas = new List<BankTransactionSubmitRequestData>();
            var data1 = new BankTransactionSubmitRequestData()
            {
                AccountNumber = "3130069520623",
                DestAccountNumber = "9210877132849961",
                BranchCode = "64",
                CreditAmount = "200",
                DebitAmount = "200",
                DocumentDate = "1403/01/20",
                DocumentNo = "123",
                NationalCode = "3770031296",
            };
            var data2 = new BankTransactionSubmitRequestData()
            {
                AccountNumber = "3130069520623",
                DestAccountNumber = "9210877132849961",
                BranchCode = "64",
                CreditAmount = "200",
                DebitAmount = "200",
                DocumentDate = "1403/01/20",
                DocumentNo = "123",
                NationalCode = "3770031296",
            };
            requestDatas.Add(data1);
            requestDatas.Add(data2);
            var testData = new BankTransactionSubmitRequest()
            {
                RequestDatas = requestDatas
            };

            var response = await _bankTransactionSubmitService.BankTransactionSubmit(testData);
            return Ok(response);

        }
    }
}
