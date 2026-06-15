using Microsoft.AspNetCore.Mvc;
using NehzatMelli.Application.Services;
using NehzatMelli.Contract.Dtos;
using NehzatMelli.Contract.Interfaces.ApplicationServices;

namespace NehzatMelli.Api.Controllers.v1
{
    public class ApplicantsController : BaseController
    {

        #region constractor
        private readonly IOutputGenerateService _outputGenerateService;
        private readonly IOutputService _outputService;
        private readonly IOutputListService _outputListService;
        public ApplicantsController( IOutputService outputService, IOutputGenerateService outputGenerateService, IOutputListService outputListService)
        {
            this._outputService = outputService;
            this._outputGenerateService = outputGenerateService;
            this._outputListService = outputListService;
        }
        #endregion


        [HttpGet]
        public async Task<IActionResult> GetOutput([FromQuery] OutputRequest request)
        {
            var response = await _outputService.GetOutput(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> OutputGenerate()
        {
            var response = await _outputGenerateService.GetOutputGenerate();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetOutputList()
        {
            var response = await _outputListService.GetOutputList();
            return Ok(response);
        }
    }
}
