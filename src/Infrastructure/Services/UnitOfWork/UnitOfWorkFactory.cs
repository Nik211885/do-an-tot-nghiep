using Application.BoundContext.BookAuthoringContext.Command;
using Application.BoundContext.BookReviewContext.Command;
using Application.BoundContext.ModerationContext.Command;
using Application.BoundContext.NotificationContext.Command;
using Application.BoundContext.OrderContext.Command;
using Application.BoundContext.UserProfileContext.Command;
using Application.Interfaces.UnitOfWork;
using Core.Interfaces;
using Infrastructure.Data.DbContext;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.UnitOfWork;

public class UnitOfWorkFactory(IServiceProvider serviceProvider)
    : IUnitOfWorkFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    public IUnitOfWork GetUnitOfWorkFor<TCommand>()
    {
        var commandType = typeof(TCommand);
        return commandType switch
        {
            { } t when ImplementsGenericInterface(t, typeof(IBookAuthoringCommand<>))
                => _serviceProvider.GetRequiredService<BookAuthoringDbContext>(),
            { } t when ImplementsGenericInterface(t,  typeof(IModerationCommand<>))
                => _serviceProvider.GetRequiredService<ModerationDbContext>(),
            { } t when ImplementsGenericInterface(t,  typeof(INotificationCommand<>))
                => _serviceProvider.GetRequiredService<NotificationDbContext>(),
            {} t when  ImplementsGenericInterface(t, typeof(IBookReviewCommand<>))
                => _serviceProvider.GetRequiredService<BookReviewDbContext>(),
            {} t when ImplementsGenericInterface(t, typeof(IUserProfileCommand<>))
                => _serviceProvider.GetRequiredService<UserProfileDbContext>(),
            {} t when ImplementsGenericInterface(t, typeof(IOrderCommand<>))
                => _serviceProvider.GetRequiredService<OrderDbContext>(),
            _ => throw new Exception("Not find unit of work make sure you has cretae and implement to factory")
        };
    }
    private static bool ImplementsGenericInterface(Type type, Type genericInterface)
    {
        return type.GetInterfaces().Any(i =>
            i.IsGenericType && i.GetGenericTypeDefinition() == genericInterface);
    }
}
