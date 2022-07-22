using May25.API.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace May25.API.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
#if RELEASE
        [Route("api/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var statusCode = HttpStatusCode.InternalServerError;

            if (context.Error is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
            }
            else if (context.Error is BadRequestException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (context.Error is ForbiddenException)
            {
                statusCode = HttpStatusCode.Forbidden;
            }

            return Problem(statusCode: (int)statusCode, title: context.Error.Message, instance: context.Path);
        }
#endif

#if DEBUG
        [Route("api/error")]
        public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException("This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var statusCode = HttpStatusCode.InternalServerError;

            if (context.Error is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
            }
            else if (context.Error is BadRequestException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (context.Error is ForbiddenException)
            {
                statusCode = HttpStatusCode.Forbidden;
            }

            return Problem(statusCode: (int)statusCode, title: context.Error.Message, detail: context.Error.StackTrace, instance: context.Path);
        }
#endif
    }
}
