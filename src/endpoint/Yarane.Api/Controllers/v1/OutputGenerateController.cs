using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NehzatMelli.Contract.Interfaces.ApplicationServices;

namespace NehzatMelli.Api.Controllers.v1
{

    public class OutputGenerateController : BaseController
    {
        #region constarctor
        private readonly IOutputGenerateService _outputGenerateService;
        public OutputGenerateController(IOutputGenerateService outputGenerateService)
        {
            this._outputGenerateService = outputGenerateService;
        }
        #endregion


        [HttpPost]
        public async Task<IActionResult> OutputGenerate()
        {
            var response = await _outputGenerateService.GetOutputGenerate();
            return Ok(response);
        }

    }
}
