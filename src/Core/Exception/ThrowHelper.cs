using System.Diagnostics.CodeAnalysis;

namespace Core.Exception;

public static class ThrowHelper
{
    public static void ThrowIfBadRequest(string message)
    {
        throw new BadRequestException(message);
    }

    public static void ThrowBadRequestWhenArgumentIsNull<T>([NotNull] T? argument, string message)
    {
        if (argument is null)
        {
            throw new BadRequestException(message);
        }
    }

    public static void ThrowBadRequestWhenArgumentNotNull<T>(T? argument, string message)
    {
        if (argument is not null)
        {
            throw new BadRequestException(message);
        }
    }
    public static void ThrowIfNotOwner()
    {
        throw new PermissionDeniedException();
    }
    
}
