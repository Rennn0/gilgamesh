namespace AspLib.RequestPipeline.Interfaces
{
    public interface IPrioritizedRequestHandler<TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse>
        where TResponse : new()
    {
        /// <summary>
        ///     priority of the handler, lower values are executed first
        /// </summary>
        int Priority { get; }
    }
}
