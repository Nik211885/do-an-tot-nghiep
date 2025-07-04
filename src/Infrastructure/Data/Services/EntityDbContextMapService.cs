using System.Reflection;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Services;

public class EntityDbContextMapService : IEntityDbContextMapService
{
    private readonly Dictionary<Type, Type> _map;

    public EntityDbContextMapService()
    {
        _map = BuildEntityToDbContextMap();
    }

    private Dictionary<Type, Type> BuildEntityToDbContextMap()
    {
        var result = new Dictionary<Type, Type>();
        var dbContextTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t=>t.IsClass && typeof(BaseDbContext).IsAssignableFrom(t));
        foreach (var dbContext in dbContextTypes)
        {
            // Db set is  generic type
            var dbSetType = dbContext.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(b=>b.PropertyType.IsGenericType 
                && b.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(p=>p.PropertyType.GetGenericArguments()[0]);
            // type is generic in db set for entity is key and value is db context for entity
            foreach (var dbType in dbSetType)
            {
                result.TryAdd(dbType, dbContext);
            }
        }
        return result;
    }

    public Type GetDbContextTypeForEntity<TEntity>()
    {
        return _map.TryGetValue(typeof(TEntity), out var dbContextType)
            ? dbContextType
            : throw new InvalidOperationException($"No DbContext found for entity: {typeof(TEntity).Name}");

    }
}
