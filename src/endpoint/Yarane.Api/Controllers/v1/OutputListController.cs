using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NehzatMelli.Contract.Interfaces.ApplicationServices;

namespace NehzatMelli.Api.Controllers.v1
{
    public class OutputListController : BaseController
    {
        #region constractor
        private readonly IOutputListService _outputListService;
        public OutputListController(IOutputListService outputListService)
        {
            this._outputListService = outputListService;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetOutputList()
        {
            var response = await _outputListService.GetOutputList();
            return Ok(response);
        }
    }
}
