using AspLib.RequestPipeline.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AspLib.RequestPipeline.Factory
{
    public class RequestHandlerFactory : IRequestHandlerFactory
    {
        private readonly IServiceProvider _provider;

        public RequestHandlerFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IRequestHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>()
            where TResponse : new() =>
            _provider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        public IRequestPipeline<TRequest, TResponse> GetPipeline<TRequest, TResponse>() =>
            _provider.GetRequiredService<IRequestPipeline<TRequest, TResponse>>();
    }
}
