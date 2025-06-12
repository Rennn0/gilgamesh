using AspLib.RequestPipeline.Exceptions;
using Microsoft.AspNetCore.Http;

namespace AspLib.RequestPipeline
{
    public class PipelineContext<TRequest, TResponse>
        where TResponse : new()
    {
        public readonly HttpContext HttpContext;

        public readonly TRequest Request;
        public TResponse Response { get; set; }
        public bool ContinueExecution { get; set; } = false;
        public LinkedList<PipelineAggregatedError> AggregatedErrors { get; private set; }

        public PipelineContext(TRequest request, HttpContext httpContext)
        {
            Request = request;
            HttpContext = httpContext;
            Response = new TResponse();
            AggregatedErrors = [];
        }

        public bool HasError => AggregatedErrors.Count > 0;

        public void AddError(PipelineAggregatedError error)
        {
            AggregatedErrors.AddLast(error);
        }
    }
}
