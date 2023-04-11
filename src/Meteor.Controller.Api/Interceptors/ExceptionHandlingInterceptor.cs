using System.Text.Json;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Meteor.Common.Core.Exceptions;

namespace Meteor.Controller.Api.Interceptors;

public class ExceptionHandlingInterceptor : Interceptor
{
    private readonly ILogger<ExceptionHandlingInterceptor> _logger;

    public ExceptionHandlingInterceptor(ILogger<ExceptionHandlingInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation
    )
    {
        try
        {
            return await continuation(request, context);
        }
        catch (MeteorNotFoundException e)
        {
            _logger.LogInformation(e, "Not found error returned for the following request: {0}", request);
            throw new RpcException(new Status(StatusCode.NotFound, e.Message));
        }
        catch (MeteorException e)
        {
            _logger.LogInformation(e, "Error returned for the following request: {0}", request);
            throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message, e));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to handle request: {0}", JsonSerializer.Serialize(request));
            throw new RpcException(new Status(StatusCode.Unknown, e.Message, e));
        }
    }
}