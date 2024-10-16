using System.Net;

namespace Mmicovic.RPSSL.API.Exceptions
{
    internal class HttpNotFoundException(string? message = null)
        : HttpException(HttpStatusCode.NotFound, message)
    { }
}
