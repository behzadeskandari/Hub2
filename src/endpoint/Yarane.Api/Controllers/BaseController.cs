using Microsoft.AspNetCore.Mvc;

namespace NehzatMelli.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
