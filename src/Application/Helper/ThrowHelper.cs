using Core.Exception;

namespace Application.Helper;

public static class ThrowHelper
{
    public static void ThrowIfBadRequest(string message)
    {
        throw new BadRequestException(message);
    }
}
