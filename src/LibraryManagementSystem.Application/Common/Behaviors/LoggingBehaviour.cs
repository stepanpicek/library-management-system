using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Common.Behaviors;

public class LoggingBehaviour<TRequest, TResponse>(
    ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        logger.LogInformation(
            "Processing request {RequestName}: {Request}",
            requestName,
            request);

        try
        {
            var response = await next();
            logger.LogInformation(
                "Completed request {RequestName}",
                requestName);
            return response;
        }
        catch(Exception ex)
        {
            logger.LogError(ex,
                "Completed request {RequestName} with error",
                requestName);
            throw;
        }
    }
}
