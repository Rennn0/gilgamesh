using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Core.Guards;

public class Guard
{
    public static T AgainstNull<T>(
        [NotNull] T? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null
    )
        where T : class
    {
        if (argument == null)
        {
            throw new ArgumentNullException(paramName);
        }

        return argument;
    }
}
