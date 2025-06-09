namespace Infrastructure.Data.Services;

public interface IEntityDbContextMapService
{
    /// <summary>
    ///  Get db context type for type of param entity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    Type GetDbContextTypeForEntity<TEntity>();
}
