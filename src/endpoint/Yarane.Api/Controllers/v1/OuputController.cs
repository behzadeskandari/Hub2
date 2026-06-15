using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NehzatMelli.Common.ReturnResults;
using NehzatMelli.Contract.Dtos;
using NehzatMelli.Contract.Interfaces.ApplicationServices;

namespace NehzatMelli.Api.Controllers.v1
{
    public class OuputController : BaseController
    {
        #region constractor
        private readonly IOutputService _outputService;
        public OuputController(IOutputService outputService)
        {
            this._outputService = outputService;
        }
        #endregion


        [HttpGet]
        public async Task<IActionResult> GetOutput([FromQuery] OutputRequest request)
        {
            var response = await _outputService.GetOutput(request);
            return Ok(response);
        }

    }
}
