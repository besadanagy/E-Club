namespace E_Club.Errors
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger; // بنستخدم _ بدل same name

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // تسجيل الخطأ مع تفاصيل أكثر
            _logger.LogError(exception,
                "An unhandled exception occurred. Message: {Message}, StackTrace: {StackTrace}",
                exception.Message,
                exception.StackTrace);

            // تحديد نوع الخطأ واختيار الـ Status Code المناسب
            var (statusCode, title) = GetExceptionDetails(exception);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
            };

            // إضافة تفاصيل إضافية في وضع Development
            if (httpContext.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() == true)
            {
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails,
                cancellationToken: cancellationToken);

            return true;
        }

        private static (int StatusCode, string Title) GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                InvalidOperationException => (StatusCodes.Status409Conflict, "Conflict"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };
        }
    }
}