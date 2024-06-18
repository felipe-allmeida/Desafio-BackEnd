using BikeRental.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace BikeRental.API.Infrastructure.Filters
{
    public class HttpExceptionFilter : IExceptionFilter
    {
        public HttpExceptionFilter()
        {
        }

        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case DomainException:
                    HandleDomainExceptions(context);
                    break;
                case NotFoundException:
                    HandleNotFoundException(context);
                    break;
                case ConflictException:
                    HandleConflictException(context);
                    break;
                case UnauthorizedException:
                    HandleUnauthorizedException(context);
                    break;
                case ForbiddenException:
                    HandleForbiddenException(context);
                    break;
                default:
                    context.ExceptionHandled = false;
                    return;
            }
        }

        private static void HandleDomainExceptions(ExceptionContext context)
        {
            var problemDetails = new ValidationProblemDetails()
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details."
            };

            problemDetails.Errors.Add(context.Exception.GetType().Name, GetDomainErrors((DomainException)context.Exception));

            context.Result = new BadRequestObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var problemDetails = new ValidationProblemDetails()
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status404NotFound,
                Detail = "Please refer to the errors property for additional details."
            };

            problemDetails.Errors.Add("Domain", [context.Exception.Message]);

            context.Result = new NotFoundObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            context.ExceptionHandled = true;
        }

        private void HandleConflictException(ExceptionContext context)
        {
            var problemDetails = new ValidationProblemDetails()
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status409Conflict,
                Detail = "Please refer to the errors property for additional details."
            };

            problemDetails.Errors.Add("Conflict", [context.Exception.Message]);

            context.Result = new ConflictObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;

            context.ExceptionHandled = true;
        }

        private void HandleUnauthorizedException(ExceptionContext context)
        {
            var problemDetails = new ValidationProblemDetails()
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status401Unauthorized,
                Detail = "Please refer to the errors property for additional details."
            };

            problemDetails.Errors.Add("Unauthorized", [context.Exception.Message]);


            context.Result = new ObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            context.ExceptionHandled = true;
        }

        private void HandleForbiddenException(ExceptionContext context)
        {
            var problemDetails = new ValidationProblemDetails()
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status403Forbidden,
                Detail = "Please refer to the errors property for additional details."
            };

            problemDetails.Errors.Add("Forbidden", [context.Exception.Message]);


            context.Result = new ObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            context.ExceptionHandled = true;
        }

        private static string[] GetDomainErrors(DomainException exception)
        {
            var errors = new List<string>();

            if (exception == null)
            {
                return [];
            }
            else
            {
                errors.Add(exception.Message);
            }

            if (exception.InnerException != null && exception.InnerException.GetType() == typeof(ValidationException))
            {
                var validationException = (ValidationException)exception.InnerException;

                errors.AddRange(validationException.Errors.Select(x => x.ErrorMessage));
            }
            else
            {
                var inner = exception.InnerException;

                while (inner != null)
                {
                    errors.Add(inner.Message);
                    inner = inner.InnerException;
                }
            }

            return [.. errors];
        }
    }
}
