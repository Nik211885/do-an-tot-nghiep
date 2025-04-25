## Add Dapper into solution make query complex
Some time I want to use sql raw make query complex or in ef expression compile
it restrict with sql because it supports many dbms or use large case, but some case I want to optimization
query with raw sql, I know in Ef new version has support method raw sql make write raw sql, but it uses in db context and feature in ef core make slowly sometimes,
in some caseI don't use feature in db context like lazy loading, tracking, or more, so I have added Dapper
in solution make query complex 

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
## Use in Repository
In repository, you get Database Connection in db context and make sure dispose connection use key work using

```csharp
    using var dbConnection = ApplicationDbContext.ReadOnly().Database().GetConnection()
```




