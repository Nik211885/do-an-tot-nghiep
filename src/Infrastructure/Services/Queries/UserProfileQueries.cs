using Application.BoundContext.UserProfileContext.Queries;
using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Models;
using Infrastructure.Data.DbContext;
using Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Queries;

public class UserProfileQueries(UserProfileDbContext userProfileDbContext) : IUserProfileQueries
{
    private readonly UserProfileDbContext _userProfileDbContext = userProfileDbContext;

    public async Task<UserProfileViewModel?> GetUserProfileByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userProfile = await _userProfileDbContext
            .UserProfiles
            .AsNoTracking()
            .Where(x=>x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return userProfile?.ToViewModel();
    }

    public async Task<PaginationItem<SearchHistoryViewModel>> GetSearchHistoryWithPaginationByUserIdAsync(Guid userId, PaginationRequest page, CancellationToken cancellationToken = default)
    {
        var searchHistory = await _userProfileDbContext
            .SearchHistories
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(s=>s.SearchDate)
            .CreatePaginationAsync(page, s => new SearchHistoryViewModel(
                s.Id,
                s.UserId,
                s.SearchTerm,
                s.SearchDate,
                s.IpAddress,
                s.SearchCout
            ), cancellationToken);
        return searchHistory;
    }

    public async Task<IReadOnlyCollection<FavoriteBookViewModel>> GetFavoriteWithBookInAndForUserIdAsync(Guid userId,CancellationToken cancellationToken = default, params Guid[] bookIds)
    {
        var favorite = await _userProfileDbContext
            .FavoriteBooks
            .Where(x => bookIds.Contains(x.FavoriteBookId)
            && x.UserId == userId)
            .ToListAsync(cancellationToken);
        return favorite.ToViewModel().ToList();
    }

    public async Task<PaginationItem<FavoriteBookViewModel>> GetFavoriteBookWithPaginationByUserIdAsync(Guid userId, 
        PaginationRequest page,
        CancellationToken cancellationToken = default)
    {
        var favorite =
            await _userProfileDbContext.FavoriteBooks
                .Where(x => x.UserId == userId)
                .CreatePaginationAsync(page, f => new FavoriteBookViewModel(
                    f.Id,
                    f.UserId,
                    f.FavoriteBookId,
                    f.CreatedOn
                ), cancellationToken);
        return favorite;
    }
}
