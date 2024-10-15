using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

using Mmicovic.RPSSL.Service;
using Mmicovic.RPSSL.API.Exceptions;

namespace Mmicovic.RPSSL.API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    /* All exceptions thrown by the API requests are routed to this controller.
     * If the exception is of custom type HttpException, specific response code and message will be sent.
     * Certain exceptions coming from the Service are known and are translated to an appropriate HttpException.
     * There are two separate handlers for production and developer environment.
     * Only when running in Dev environment will this controller give the full stack trace to the user. */
    public class ErrorHandlingController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError()
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = TranslateException(exceptionHandlerFeature!.Error);

            if (exception is HttpException ex)
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

            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = TranslateException(exceptionHandlerFeature!.Error);

            var statusCode = exception is HttpException ex
                ? ex.StatusCode
                : HttpStatusCode.InternalServerError;

            return Problem(statusCode: (int)statusCode,
                           title: exception.Message,
                           detail: exception.StackTrace);
        }

        private static Exception TranslateException(Exception exception)
        {
            // Only translate exceptions that already lost their stack trace
            if (exception is NumberGenerationUnavailableException)
                return new HttpNumberGenerationUnavailableException();

            if (exception is UserAlreadyExistsException)
                return new HttpAlreadyExistsException("User with that name already exists");

            return exception;
        }
    }
}
