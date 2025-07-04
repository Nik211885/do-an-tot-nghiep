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
    public static void ThrowNotFound(string entity)
    {
        throw new NotFoundException(entity, null);
    }
    public static void ThrowNotFoundWhenItemIsNull([NotNull]object? entity, string entityName,
        Dictionary<string, string> identifier)
    {
        if (entity is null)
        {
            throw new NotFoundException(entityName, identifier);
        }
    }
    public static void ThrowNotFoundWhenItemIsNull([NotNull]object? entity, 
        Dictionary<string, Dictionary<string, string>> identifier)
    {
        if (entity is null)
        {
            throw new NotFoundException(identifier.Keys.First(), identifier.Values.First());
        }
    }

    public static void ThrowNotFound(Dictionary<string, Dictionary<string, string>> identifier)
    {
        throw new NotFoundException(identifier.Keys.First(), identifier.Values.First());
    }

    public static void ThrowBadRequestWhenITemIsNotNull(object? obj,string message)
    {
        if (obj is not null)
        {
            throw new BadRequestException(message);
        }
    }
    [DoesNotReturn]
    public static void ThrowIfBadRequest(string message)
    {
        throw new BadRequestException(message);
    }
}
