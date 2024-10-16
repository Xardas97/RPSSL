using System.Net;

namespace Mmicovic.RPSSL.API.Exceptions
{
    internal class HttpCredentialsException(string? message = null)
        : HttpException(HttpStatusCode.Unauthorized, message)
    { }
}
