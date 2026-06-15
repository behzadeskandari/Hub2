using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Yarane.Common.ReturnResults;

namespace Yarane.Common.Exceptions
{
    public sealed class LogicException : AppException
    {
        public LogicException(ApiResultRsCode rsCode, string message)
           : base(rsCode, message, HttpStatusCode.InternalServerError)
        {

        }
    }
}
