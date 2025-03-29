using System.Net;

namespace TinyMicroblog.Application.Exceptions
{
    public class AppException : Exception
    {
        public AppException(HttpStatusCode httpStatusCode, string errorcode) : base(errorcode) { }
    }
}
