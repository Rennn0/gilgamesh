using AspLib.RequestPipeline.Exceptions;
using Microsoft.AspNetCore.Http;

namespace AspLib.RequestPipeline.Interfaces
{
    public interface IRequestPipeline<in TRequest, TResponse>
    {
        /// <summary>
        ///     uses provided http context to execute the pipeline
        /// </summary>
        /// <param name="request"></param>
        /// <param name="httpContext"></param>
        /// <exception cref="PipelineFailedException"></exception>
        /// <returns></returns>
        Task<TResponse> ExecuteAsync(TRequest request, HttpContext httpContext);

        /// <summary>
        ///     gets context from http call and pass it to the pipeline
        /// </summary>
        /// <param name="request"></param>
        /// /// <exception cref="PipelineFailedException"></exception>
        /// <returns></returns>
        Task<TResponse> ExecuteAsync(TRequest request);
    }
}
