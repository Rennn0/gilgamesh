namespace AspLib.RequestPipeline.Exceptions;

public class PipelineFailedException : Exception
{
    private readonly IReadOnlyCollection<PipelineAggregatedError>? _errors;

    public PipelineFailedException()
        : base() { }

    public PipelineFailedException(string message)
        : base(message) { }

    public PipelineFailedException(IReadOnlyCollection<PipelineAggregatedError> errors)
        : base()
    {
        _errors = errors;
    }

    public IReadOnlyCollection<PipelineAggregatedError> Errors => _errors ?? [];
}
