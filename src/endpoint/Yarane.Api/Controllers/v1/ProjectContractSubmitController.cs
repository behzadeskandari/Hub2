using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NehzatMelli.Contract.Dtos;
using NehzatMelli.Contract.Interfaces.ApplicationServices;

namespace NehzatMelli.Api.Controllers.v1
{
    public class ProjectContractSubmitController : BaseController
    {
        #region constractor
        private readonly IProjectContractSubmitService _projectContractSubmitService;
        public ProjectContractSubmitController(IProjectContractSubmitService projectContractSubmitService)
        {
            this._projectContractSubmitService = projectContractSubmitService;
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> ProjectContractSubmit([FromBody] ProjectContractSubmitRequest request)
        {
            var response = await _projectContractSubmitService.ProjectContractSubmit(request);
            return Ok(response);
        }
    }
}
