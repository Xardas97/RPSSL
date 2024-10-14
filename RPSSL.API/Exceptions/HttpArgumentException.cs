using System.Net;

namespace Mmicovic.RPSSL.API.Exceptions
{
    internal class HttpArgumentException(string? message = null)
        : HttpException(HttpStatusCode.BadRequest, message)
    { }
}
