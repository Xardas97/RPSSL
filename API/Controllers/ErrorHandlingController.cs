using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

using Mmicovic.RPSSL.API.Exceptions;

namespace Mmicovic.RPSSL.API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    /* All exceptions thrown by the API requests are routed to this controller.
     * If the exception is of custom type HttpResponseException, specific response code and message will be sent.
     * There are two separate handlers for production and developer environment.
     * Only when running in Dev environment will this controller give the full stack trace to the user. */
    public class ErrorHandlingController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError()
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            if (exceptionHandlerFeature.Error is HttpException ex)
            {
                return Problem(statusCode: (int)ex.StatusCode, title: ex.Message);
            }

            return Problem();
        }

        [Route("/error-dev")]
        public IActionResult HandleErrorDevEnvironment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                return NotFound();
            }

            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            var statusCode = HttpStatusCode.InternalServerError;
            if (exceptionHandlerFeature.Error is HttpException ex)
            {
                statusCode = ex.StatusCode;
            }

            return Problem(statusCode: (int)statusCode,
                           title: exceptionHandlerFeature.Error.Message,
                           detail: exceptionHandlerFeature.Error.StackTrace);
        }
    }
}
