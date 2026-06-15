using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NehzatMelli.Contract.Dtos;
using NehzatMelli.Contract.Interfaces.ApplicationServices;

namespace NehzatMelli.Api.Controllers.v1
{

    public class OpenBankAccountErrorSubmitController : BaseController
    {
        #region constractor
        private readonly IOpenBankAccountErrorSubmitService _openBankAccountErrorSubmitService;
        public OpenBankAccountErrorSubmitController(IOpenBankAccountErrorSubmitService openBankAccountErrorSubmitService)
        {
            this._openBankAccountErrorSubmitService = openBankAccountErrorSubmitService;
        }
        #endregion


        [HttpPost]
        public async Task<IActionResult> OpenBankAccountErrorSubmit([FromBody] OpenBankAccountErrorSubmitRequest request)
        {
            var response = await _openBankAccountErrorSubmitService.OpenBankAccountErrorSubmit(request);
            return Ok(response);

        }
    }
}
