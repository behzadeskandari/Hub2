using Microsoft.AspNetCore.Mvc;
using NehzatMelli.Contract.Dtos;
using NehzatMelli.Contract.Interfaces.ApplicationServices;

namespace NehzatMelli.Api.Controllers.v1
{
    public class AccountBlockSubmitController :BaseController
    {
        private readonly IAccountBlockSubmitService _accountBlockSubmitService;
        #region constractor
        public AccountBlockSubmitController(IAccountBlockSubmitService accountBlockSubmitService)
        {
            this._accountBlockSubmitService = accountBlockSubmitService;
        }
        #endregion


      
    }
}
