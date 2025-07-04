## Strategy load with Ef Core


## Eager Loading


## Lazy Loading


## Explicit Loading


Some case make accelerate query in database in EFCore it make use some feature in ef

- Use compile query in runtime
    ```csharp
    static Func<ApplicationDbContext, paramType, IQueryable<TEntity>> _getEntityBeParam
        = EF.CompileQuery((ApplicationDbContext context, paramType param)) 
            => context.Entity.Where(e=>e.param = param)
    ```
- Use raw query
  ```csharp
    var result = ApplicationDbContext.Entity.FromSqlRaw("SELECT * FROM Entity").ToList()
    ```
- Some case with Include you can use <code>SplitQuery</code> when use case it don't use join key word in sql, it split more query and compile them and pass to result


If you have some many condition and reuse many times like soft delete you have can use feature filter




## Setup with EF core
### First create migrations
If your dbContext has dependency to services container you should create new instance for dbContext with DbContextDesignTimeFactory because in time run migration, services container not
```csharp
public class YourDbContextDesignTimedFactory : IDesignTimeDbContextFactory<YourdbContext>{
    public YourDbContext CreatedbContext()
    {
        // Create services for your db context and pass it to constructer
        return new YourDbContext();
    }
}
```

```csharp
dotnet ef migrations add [migrations message] --project [Project constains dbContext] --startup-project [Project constains startup file] --context [Your dbContext] --output-dir [Folder will constains migrations]
```

## Migrations to database


```csharp
  dotnet ef database update --context [Your dbContext] --project [Project constains your DbContext] --startup-project [Project constains startup file]
```

##  Cache Stamp


