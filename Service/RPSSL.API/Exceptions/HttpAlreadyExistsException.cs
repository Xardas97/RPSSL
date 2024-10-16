using System.Net;

namespace Mmicovic.RPSSL.API.Exceptions
{
    // BadRequest suggested instead of Conflict when it comes to input validation
    internal class HttpAlreadyExistsException(string? message = null)
        : HttpException(HttpStatusCode.BadRequest, message)
    { }
}
