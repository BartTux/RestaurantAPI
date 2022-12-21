using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
	private readonly ILogger<ErrorHandlingMiddleware> _logger;

	public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
	{
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
		try 
		{
			await next.Invoke(context);
		}
        catch (NotFoundException ex)
        {
			await ReturnResponse(context, StatusCodes.Status404NotFound, ex.Message);
        }
		catch (BadRequestException ex)
		{
            await ReturnResponse(context, StatusCodes.Status400BadRequest, ex.Message);
        }
		catch (ForbidException ex)
		{
			context.Response.StatusCode = StatusCodes.Status403Forbidden;
		}
        catch (Exception ex) 
		{
			_logger.LogError(ex, ex.Message);
            await ReturnResponse(context, StatusCodes.Status500InternalServerError, "Something went wrong...");
        }
    }

	private async Task ReturnResponse(HttpContext httpContext, int statusCode, string message)
	{
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(message);
    }
}
