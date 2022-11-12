using System.Diagnostics;

namespace RestaurantAPI.Middleware;

public class RequestTimeMiddleware : IMiddleware
{
    private Stopwatch _stopwatch;
    private readonly ILogger<RequestTimeMiddleware> _logger;

    public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
    {
        _logger = logger;
        _stopwatch = new Stopwatch();
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _stopwatch.Start();
        await next.Invoke(context);
        _stopwatch.Stop();

        var elapsedSeconds = _stopwatch.ElapsedMilliseconds / 1000;

        if (elapsedSeconds > 4)
        {
            var message = $"{ context.Request.Method } at { context.Request.Path } took { elapsedSeconds } sec.";
            _logger.LogInformation(message);
        }
    }
}
