using Application.Interfaces.CQRS;

namespace Application.BoundContext.UserProfileContext.Command;

public interface IUserProfileCommand<TResponse>
    : ICommand<TResponse>;
