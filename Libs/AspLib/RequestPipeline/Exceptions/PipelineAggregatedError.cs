namespace AspLib.RequestPipeline.Exceptions
{
    public class PipelineAggregatedError
    {
        public readonly string StackTrace;
        public readonly string Message;
        public readonly string Property;
        public readonly string Hint;

        public PipelineAggregatedError(
            string message,
            string stackTrace,
            string property,
            string hint
        )
        {
            Message = message;
            StackTrace = stackTrace;
            Property = property;
            Hint = hint;
        }

        public PipelineAggregatedError(string message, string stackTrace)
        {
            Message = message;
            StackTrace = stackTrace;
            Property = string.Empty;
            Hint = string.Empty;
        }

        public PipelineAggregatedError(string message)
        {
            Message = message;
            StackTrace = string.Empty;
            Property = string.Empty;
            Hint = string.Empty;
        }
    }
}
