using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NehzatMelli.Application.Services;
using NehzatMelli.Common.ReturnResults;
using NehzatMelli.Contract.Dtos;
using NehzatMelli.Contract.Interfaces.ApplicationServices;

namespace NehzatMelli.Api.Controllers.v1
{
    public class StatusInquiryController : BaseController
    {
        #region constarctor
        private readonly IStatusInquiryService _statusInquiryService;
        public StatusInquiryController(IStatusInquiryService statusInquiryService)
        {
            this._statusInquiryService = statusInquiryService;
        }
        #endregion

        [HttpGet]
        public virtual async Task<IActionResult> GetStatusInquiry([FromQuery] StatusInquiryRequest statusInquiryRequest)
        {
            var response = await _statusInquiryService.GetStatusInquiry(statusInquiryRequest);
            return Ok(response);
        }


    }
}
