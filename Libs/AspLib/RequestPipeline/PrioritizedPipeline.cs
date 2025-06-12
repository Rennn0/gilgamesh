using AspLib.RequestPipeline.Exceptions;
using AspLib.RequestPipeline.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AspLib.RequestPipeline
{
    public class PrioritizedPipeline<TRequest, TResponse> : IRequestPipeline<TRequest, TResponse>
        where TResponse : new()
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEnumerable<IPrioritizedRequestHandler<TRequest, TResponse>> _handlers;

        public PrioritizedPipeline(
            IEnumerable<IPrioritizedRequestHandler<TRequest, TResponse>> handlers,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _handlers = handlers.OrderBy(h => h.Priority);
        }

        public async Task<TResponse> ExecuteAsync(TRequest request, HttpContext httpContext)
        {
            PipelineContext<TRequest, TResponse> context = new PipelineContext<TRequest, TResponse>(
                request,
                httpContext
            );

            foreach (IPrioritizedRequestHandler<TRequest, TResponse> handler in _handlers)
            {
                try
                {
                    await handler.ExecuteAsync(context);
                }
                catch (HandlerException he)
                {
                    context.AggregatedErrors.AddLast(
                        new PipelineAggregatedError(
                            he.Message,
                            he.StackTrace ?? string.Empty,
                            he.Property,
                            he.Hint
                        )
                    );
                }
                catch (Exception e)
                {
                    context.AggregatedErrors.AddLast(
                        new PipelineAggregatedError(
                            e.StackTrace ?? string.Empty,
                            $"Handler {handler.GetType().Name} failed: {e.Message}"
                        )
                    );
                }

                CheckContext(context);
            }

            // TODO: logging
            return context.Response ?? new TResponse();
        }

        public Task<TResponse> ExecuteAsync(TRequest request) =>
            ExecuteAsync(request, _httpContextAccessor.HttpContext ?? new DefaultHttpContext());

        /// <summary>
        /// </summary>
        /// <exception cref="PipelineFailedException"></exception>
        /// <param name="context"></param>
        private void CheckContext(PipelineContext<TRequest, TResponse> context)
        {
            if (context is { HasError: true, ContinueExecution: false })
                throw new PipelineFailedException(context.AggregatedErrors);
        }
    }
}
