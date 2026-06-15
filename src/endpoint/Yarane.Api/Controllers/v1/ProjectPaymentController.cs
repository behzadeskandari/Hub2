using Microsoft.AspNetCore.Mvc;
using NehzatMelli.Common.Utilities;
using NehzatMelli.Contract.Interfaces.ApplicationServices;

namespace NehzatMelli.Api.Controllers.v1
{
    public class ProjectPaymentController : BaseController
    {
        #region constarctor
        private readonly IProjectPaymentService _projectPaymentService;
        public ProjectPaymentController(IProjectPaymentService projectPaymentService)
        {
            this._projectPaymentService = projectPaymentService;
        }
        #endregion

        [HttpGet]
        public virtual async Task<IActionResult> GetProjectPayment()
        {
            var response = await _projectPaymentService.GetProjectPayment();
            return Ok(response);
        }
    }
}
