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
            "Processing request {RequestName}",
            requestName);

        try
        {
            var response = await next();
            logger.LogInformation(
                "Completed request {RequestName}",
                requestName);
            return response;
        }
        catch
        {
            logger.LogError(
                "Completed request {RequestName} with error",
                requestName);
            throw;
        }
    }
}
