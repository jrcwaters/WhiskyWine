using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WhiskyWine.BottleService.API.Controllers
{
    /// <summary>
    /// The global error handler for the BottleService.
    /// </summary>
    [ApiController]
    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// Handles exceptions in development environments. Provides detailed information about the error including a stack trace.
        /// </summary>
        /// <param name="webHostEnvironment">The environment in which the system is running. This method is only appropraite for development.</param>
        /// <returns>IActionResult containing Problem with detailed error information.</returns>
        [Route("/error-local-development")]
        public IActionResult ErrorLocalDevelopment(
        [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "Do not invoke this method in production environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }

        /// <summary>
        /// Handles exceptions in non-production environments.
        /// </summary>
        /// <returns>IActionResult containing Problem with production appropriate error information.</returns>
        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}
