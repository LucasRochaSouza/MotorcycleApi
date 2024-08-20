using System.Diagnostics;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        _logger.LogTrace("Handling request: {Method} {Path}", request.Method, request.Path);

        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        var response = context.Response;
        stopwatch.Stop();
        _logger.LogTrace("Handled request: {Method} {Path} responded with {StatusCode} in {ElapsedMilliseconds}ms",
            request.Method, request.Path, response.StatusCode, stopwatch.ElapsedMilliseconds);
    }
}