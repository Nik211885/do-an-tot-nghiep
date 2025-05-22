using System.Diagnostics.CodeAnalysis;

namespace Application.Exceptions;

public static class ThrowHelper
{
    public static void ThrowWhenUserForbidden()
    {
        throw new ForbiddenException();
    }

    public static void ThrowWhenUserUnauthorized()
    {
        throw new UnauthorizedAccessException();
    }

    public static void ThrowNotFoundWhenItemIsNull([NotNull] object? entity, string entityName)
    {
        if (entity is null)
        {
            throw new NotFoundException(entityName, null);
        }
    }
    
    public static void ThrowNotFoundWhenItemsIsNull([NotNull]object? entity, string entityName,
        Dictionary<string, string> identifier)
    {
        if (entity is null)
        {
            throw new NotFoundException(entityName, identifier);
        }
    }
}
