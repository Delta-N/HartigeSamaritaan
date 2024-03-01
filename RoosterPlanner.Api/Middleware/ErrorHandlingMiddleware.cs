using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using CorrelationId.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Api.Extensions;
namespace RoosterPlanner.Api.Middleware
{
    /// <summary>
    /// Middleware for handling errors
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        /// <summary>
        /// Logging context
        /// </summary>
        public object LogContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="correlationContextAccessor">The context to get the correlation id from.</param>
        public ErrorHandlingMiddleware(RequestDelegate next, ICorrelationContextAccessor correlationContextAccessor)
        {
            _next = next;
            _correlationContextAccessor = correlationContextAccessor;
        }

        /// <summary>
        /// Invokes the Error handling context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception,
        ILogger<ErrorHandlingMiddleware> logger)
        {
            var (status, message) = GetErrorMessage(exception);

            var problemDetails = new ProblemDetails
            {
            Instance = _correlationContextAccessor.CorrelationContext.CorrelationId,
            Detail = "The instance value (correlation id) should be used to investigate the problem."
            };

            logger.LogError(exception, message);

            problemDetails.Title = message;
            problemDetails.Status = (int)status;

            if (status == HttpStatusCode.BadRequest)
            {
                problemDetails.Detail = exception.Message;
            }

#if DEBUG
            problemDetails.Extensions.Add(exception.Message, exception.StackTrace);
#endif

            if (exception is ValidationException validationException)
            {
                AddExtensionData(problemDetails.Extensions, validationException);
            }

            return WriteProblemDetailsAsync(context.Response, problemDetails);
        }

        private static void AddExtensionData(IDictionary<string, object> extensions,
        ValidationException validationException)
        {
            var i = 0;
            foreach (var error in validationException.Errors)
            {
                var errorMessage =
                $"{error.ErrorMessage} - Invalid provided value for {error.PropertyName}: {error.AttemptedValue}";
                extensions.Add(i++.ToString(), errorMessage);
            }
        }

        private static (HttpStatusCode status, string message) GetErrorMessage(Exception exception)
        {
            return exception switch
            {
            BadHttpRequestException badRequest => ((HttpStatusCode)badRequest.StatusCode, "Invalid request"),
            ValidationException => (HttpStatusCode.BadRequest, "Resource is invalid"),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access"),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
            };
        }

        private static Task WriteProblemDetailsAsync(HttpResponse response, ProblemDetails problemDetails)
        {
            response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
            return response.WriteJsonAsync(problemDetails, "application/problem+json");
        }
    }
}
