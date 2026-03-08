using Grpc.Core;

namespace SqlViewer.ApiGateway.Middleware;

public sealed class GrpcExceptionMiddleware(ILogger<GrpcExceptionMiddleware> logger, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (RpcException ex)
        {
            logger.LogWarning("gRPC Error: {StatusDetail}, Code: {StatusCode}, Path: {Path}",
                ex.Status.Detail, ex.StatusCode, context.Request.Path);

            (int httpStatus, string defaultMessage) = ex.StatusCode switch
            {
                StatusCode.Unauthenticated => (StatusCodes.Status401Unauthorized, "Authorization error"),
                StatusCode.InvalidArgument => (StatusCodes.Status400BadRequest, "Incorrect data"),
                StatusCode.PermissionDenied => (StatusCodes.Status403Forbidden, "Insufficient rights"),
                StatusCode.NotFound => (StatusCodes.Status404NotFound, "Resource not found"),
                _ => (StatusCodes.Status500InternalServerError, "Internal service error")
            };

            string errorMessage = !string.IsNullOrWhiteSpace(ex.Status.Detail)
                ? ex.Status.Detail
                : defaultMessage;

            context.Response.StatusCode = httpStatus;
            await context.Response.WriteAsJsonAsync(new { message = errorMessage });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error in API Gateway at {Path}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = "An unexpected error occurred" });
        }
    }
}
