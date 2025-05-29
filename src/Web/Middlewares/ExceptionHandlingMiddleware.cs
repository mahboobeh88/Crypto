using System.Net;
using System.Text.Json;

namespace Crypto.Web.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // اجرا ادامه پیدا می‌کنه
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode status;
        string message;

        switch (ex)
        {
            case KeyNotFoundException:
                status = HttpStatusCode.NotFound;
                message = ex.Message;
                break;

            case HttpRequestException:
                status = HttpStatusCode.ServiceUnavailable;
                message = ex.Message;
                break;

            case FormatException:
                status = HttpStatusCode.BadRequest;
                message = ex.Message;
                break;


            case ArgumentNullException:
                status = HttpStatusCode.InternalServerError;
                message = ex.Message;
                break;

            case ArgumentException:
                status = HttpStatusCode.BadRequest;
                message = ex.Message;
                break;


            default:
                status = HttpStatusCode.InternalServerError;
                message = "Exception Happend.For more details open log file";
                _logger.LogError(ex, "Unhandled exception.");
                break;
        }

        context.Response.StatusCode = (int)status;

        var result = JsonSerializer.Serialize(new
        {
            error = message,
            status = (int)status
        });

        return context.Response.WriteAsync(result);
    }
}
