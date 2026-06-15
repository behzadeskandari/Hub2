using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yarane.Common.Messages
{
    public static class ErrorMessages
    {
        public const string Invalid = "Invalid request.";
        public const string InvalidUser = "InvalidUser";
        public const string InvalidPassword = "InvalidPassword";
        public const string DuplicateEmail = "DuplicateEmail";
        public const string UserNotFound = "UserNotFound";
        public const string ErrorInLogout = "ErrorInLogout";
        public const string AccountCraetionFailed = "AccountCraetionFailed";
    }


    public static class SuccessMessages
    {
        public const string Success = "Success.";
        public const string LoginSuccess = "LoginSuccess.";
        public const string LogOutSucess = "LogOutSucess.";
        public const string AccountCreated = "AccountCreated.";
    }
}
