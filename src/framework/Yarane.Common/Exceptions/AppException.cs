using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Yarane.Common.ReturnResults;

namespace Yarane.Common.Exceptions
{
    public abstract class AppException : Exception
    {
        public ApiResultRsCode RsCode { get; set; }
        public string Message { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public AppException(ApiResultRsCode rsCode, string message, HttpStatusCode httpStatusCode)
        {
            RsCode = rsCode;
            Message = message;
            HttpStatusCode = httpStatusCode;
        }
    }
}
