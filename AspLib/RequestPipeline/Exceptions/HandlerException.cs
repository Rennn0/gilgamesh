namespace AspLib.RequestPipeline.Exceptions;

public class HandlerException : Exception
{
    public readonly string Property;
    public readonly string Hint;

    public HandlerException()
    {
        Property = string.Empty;
        Hint = string.Empty;
    }

    public HandlerException(string message)
        : base(message)
    {
        Property = string.Empty;
        Hint = string.Empty;
    }

    public HandlerException(string message, string property)
        : base(message)
    {
        Property = property;
        Hint = string.Empty;
    }

    public HandlerException(string message, string property, string hint)
        : base(message)
    {
        Property = property;
        Hint = hint;
    }
}
