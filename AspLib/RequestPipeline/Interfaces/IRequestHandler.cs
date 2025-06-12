using AspLib.RequestPipeline.Exceptions;

namespace AspLib.RequestPipeline.Interfaces
{
    public interface IRequestHandler<TRequest, TResponse>
        where TResponse : new()
    {
        /// <summary>
        ///    this is the method that will be called  without context
        /// </summary>
        /// <param name="request"></param>
        /// /// <exception cref="HandlerException"></exception>
        /// <returns></returns>
        Task<TResponse> ExecuteAsync(TRequest request);

        /// <summary>
        ///     this is the method that will be called by the pipeline and contain context
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="HandlerException"></exception>
        /// <returns></returns>
        Task ExecuteAsync(PipelineContext<TRequest, TResponse> context);
    }
}
