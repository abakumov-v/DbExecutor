# DbExecutor

Package|Last version
-|-
DbConn.DbExecutor.Abstract|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/DbConn.DbExecutor.Abstract.svg)](https://www.nuget.org/packages/DbConn.DbExecutor.Abstract/)
DbConn.DbExecutor.Dapper|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/DbConn.DbExecutor.Dapper.svg)](https://www.nuget.org/packages/DbConn.DbExecutor.Dapper/)
DbConn.DbExecutor.Dapper.Ioc.Autofac|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/DbConn.DbExecutor.Dapper.Ioc.Autofac.svg)](https://www.nuget.org/packages/DbConn.DbExecutor.Dapper.Ioc.Autofac/)

Simple wrapper with factory for working with database connections (for example, Dapper)

## Builds

Branch|Build status
-|-
master|[![Build status](https://ci.appveyor.com/api/projects/status/2fsh97gw8nrw4wj0/branch/master?svg=true)](https://ci.appveyor.com/project/Valeriy1991/dbexecutor-qb91d/branch/master)
dev|[![Build status](https://ci.appveyor.com/api/projects/status/9mk8efhqwqqibgt5/branch/dev?svg=true)](https://ci.appveyor.com/project/Valeriy1991/dbexecutor/branch/dev)



AppVeyor Nuget project feed: 
https://ci.appveyor.com/nuget/dbexecutor-q2ir84d55gwi

## Dependencies

Project|Dependency
-|-
DbConn.DbExecutor.Abstract|*No*
DbConn.DbExecutor.Dapper|[Dapper](https://github.com/StackExchange/Dapper)
DbConn.DbExecutor.Dapper.Ioc.Autofac|[Autofac.Extensions.DependencyInjection](https://github.com/autofac/Autofac.Extensions.DependencyInjection)

## How to use

### 1. Abstractions

1. Install from NuGet:
```
Install-Package DbConn.DbExecutor.Abstract
```
2. Then you can use `IDbExecutor` and `IDbExecutorFactory` interfaces.


### 2. Implementations

#### 2.1. Dapper

1. Install from NuGet:
```
Install-Package DbConn.DbExecutor.Dapper
```

2. After you can use `DapperDbExecutor` and `DapperDbExecutorFactory` 
classes. For example (business component that creates user):
```csharp
public class UserCreator
{
    private readonly IDbExecutorFactory _dbExecutorFactory;
    private readonly AppConfig _appConfig;

    public UserCreator(IDbExecutorFactory dbExecutorFactory, IOptions<AppConfig> appConfigOptions)
    {
        if(dbExecutorFactory == null)
            throw new ArgumentNullException(nameof(dbExecutorFactory));
        _dbExecutorFactory = dbExecutorFactory;

        if (appConfigOptions?.Value == null)
            throw new ArgumentNullException(nameof(appConfigOptions));
        if (appConfigOptions.Value.ConnectionStrings == null)
            throw new ArgumentNullException(nameof(appConfigOptions), "Connection strings section in configuration file is null");
        _appConfig = appConfigOptions.Value;
    }

    public void CreateUser(string email, string password)
    {
        List<SomeEntity> someEntities;
        
        // Example for non-transactional IDbExecutor:
        using (var dbExecutor = _dbExecutorFactory.Create(_appConfig.ConnectionStrings.UserDb))
        {
            var querySql = $"select * from ...";
            someEntities = dbExecutor.Query<SomeEntity>().ToList();                
        }

        // Example for transactional IDbExecutor:
        using (var dbExecutor = _dbExecutorFactory.CreateTransactional(_appConfig.ConnectionStrings.UserDb))
        {
            try
            {
                // Some logic for creating user

                // Some strange logic :)
                var executeSql = $"exec dbo.SomeStrangeProcedure @param1 = 1 ...";
                dbExecutor.Execute(executeSql);

                // Commit for applying our changes because IDbExecutor was created with opening transaction:
                dbExecutor.Commit();
            }
            catch(Exception ex)
            {
                // Log error if you need

                dbExecutor.Rollback();
            }
        }
    }
}
```
`AppConfig` example:
```csharp
public class AppConfig
{
  public ConnectionConfig ConnectionStrings { get; set; }
}
// ...
public class ConnectionConfig
{  
  public string UserDb { get; set; }
}
```
And `appsettings.json`:
```json
{
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Trace",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "ConnectionStrings": {
    "UserDb": "data source=...;initial catalog=...;User ID=...;Password=...;"
  }
}

```

### 2.2 Dependency injection & IoC

#### Autofac
1. Install from NuGet:
```
Install-Package DbConn.DbExecutor.Dapper.Ioc.Autofac
```
2. This package have an Autofac registration module with name `DbExecutorRegistrationModule`. Just create new instanse of this class when you call `RegisterModule` method of Autofac `ContainerBuilder`:
```csharp
builder.RegisterModule(new DbExecutorRegistrationModule());
```
