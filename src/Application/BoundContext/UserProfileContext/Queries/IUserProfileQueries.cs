using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Interfaces.Query;
using Application.Models;

namespace Application.BoundContext.UserProfileContext.Queries;

public interface IUserProfileQueries : IApplicationQueryServicesExtension
{
    // user
    Task<UserProfileViewModel?> GetUserProfileByIdAsync(Guid id, CancellationToken cancellationToken = default);
    // Search History
    
    Task<PaginationItem<SearchHistoryViewModel>> GetSearchHistoryWithPaginationByUserIdAsync(Guid userId, PaginationRequest page, CancellationToken cancellationToken = default);
    
    // Favorite book
    Task<IReadOnlyCollection<FavoriteBookViewModel>> GetFavoriteWithBookInAndForUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default,
        params Guid[] bookIds);

    Task<PaginationItem<FavoriteBookViewModel>> GetFavoriteBookWithPaginationByUserIdAsync(Guid userId,
        PaginationRequest page,
        CancellationToken cancellationToken = default);
}
