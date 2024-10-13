using System.Net;

namespace Mmicovic.RPSSL.API.Exceptions
{
    public class HttpException(HttpStatusCode statusCode, string? message = null) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; } = statusCode;
    }
}
