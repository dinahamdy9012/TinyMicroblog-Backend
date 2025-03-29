using System.Net;

namespace TinyMicroblog.SharedKernel.Exceptions
{
    public class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public AppException(HttpStatusCode httpStatusCode, string errorcode) : base(errorcode) 
        { 
            StatusCode = httpStatusCode;
        }
    }
}
