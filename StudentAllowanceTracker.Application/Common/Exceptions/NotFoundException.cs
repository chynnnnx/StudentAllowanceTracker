using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Common.Exceptions
{
    public class NotFoundException: Exception
    {
        public HttpStatusCode StatusCode { get; }
        public NotFoundException(string message, HttpStatusCode statusCode = HttpStatusCode.NotFound) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
