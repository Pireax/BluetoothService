using Microsoft.AspNetCore.Diagnostics;

namespace BluetoothService;

public class LoggingExceptionHandler(ILogger<LoggingExceptionHandler> logger) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionMessage = exception.Message;
        logger.LogError(
            "Exception caught at {time}, Exception: {exceptionMessage}", DateTime.UtcNow, exceptionMessage);
        return ValueTask.FromResult(false);
    }
}
