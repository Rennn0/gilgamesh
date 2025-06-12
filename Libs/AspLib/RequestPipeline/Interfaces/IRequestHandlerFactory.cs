namespace AspLib.RequestPipeline.Interfaces
{
    public interface IRequestHandlerFactory
    {
        /// <summary>
        ///     returns SINGLE handler for the request type.
        ///     for some reason if u decide to call GetHandler and then pass Pipeline context it will just execute single handler randomly.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        IRequestHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>()
            where TResponse : new();

        /// <summary>
        ///     retusns a pipeline for the request type, handlers will be executet using priority
        ///     <see cref="IPrioritizedRequestHandler{TRequest,TResponse}"/>
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        IRequestPipeline<TRequest, TResponse> GetPipeline<TRequest, TResponse>();
    }
}
