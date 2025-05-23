using System.Diagnostics.CodeAnalysis;
using Core.Exception;

namespace Application.Exceptions;

internal static class ThrowHelper
{
    public static void ThrowIfNotOwner()
    {
        throw new PermissionDeniedException();
    }

    public static void ThrowNotFoundWhenItemIsNull([NotNull] object? entity, string entityName)
    {
        if (entity is null)
        {
            throw new NotFoundException(entityName, null);
        }
    }
    
    public static void ThrowNotFoundWhenItemIsNull([NotNull]object? entity, string entityName,
        Dictionary<string, string> identifier)
    {
        if (entity is null)
        {
            throw new NotFoundException(entityName, identifier);
        }
    }
}
