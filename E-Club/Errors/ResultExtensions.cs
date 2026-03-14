namespace E_Club.Errors;

public static class ResultExtensions
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert success result to a problem");

        var statusCode = result.Error.StatusCode ?? StatusCodes.Status400BadRequest;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitleForStatusCode(statusCode),
            Detail = result.Error.Description,
            Type = GetTypeForStatusCode(statusCode),
            Extensions = new Dictionary<string, object?>
            {
                ["errors"] = new[]
                {
                    new
                    {
                        code = result.Error.Code,
                        description = result.Error.Description
                    }
                }
            }
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }

    public static ObjectResult ToProblem<T>(this Result<T> result)
    {
        return ((Result)result).ToProblem();
    }

    private static string GetTitleForStatusCode(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        409 => "Conflict",
        422 => "Unprocessable Entity",
        500 => "Internal Server Error",
        _ => "Error"
    };

    private static string GetTypeForStatusCode(int statusCode) => statusCode switch
    {
        400 => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        401 => "https://tools.ietf.org/html/rfc7235#section-3.1",
        403 => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
        404 => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        409 => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
        422 => "https://tools.ietf.org/html/rfc4918#section-11.2",
        500 => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        _ => "about:blank"
    };
}