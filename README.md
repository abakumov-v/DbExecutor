# DbExecutor

Simple wrapper with its factory for working with database connections (for example, Dapper)

## Builds

Branch|Build status
-|-
master|[![Build status](https://ci.appveyor.com/api/projects/status/9mk8efhqwqqibgt5?svg=true)](https://ci.appveyor.com/project/Valeriy1991/dbexecutor)
dev|[![Build status](https://ci.appveyor.com/api/projects/status/9mk8efhqwqqibgt5/branch/dev?svg=true)](https://ci.appveyor.com/project/Valeriy1991/dbexecutor/branch/dev)



## AppVeyor Nuget project feed

https://ci.appveyor.com/nuget/dbexecutor-q2ir84d55gwi

## How to use

### Abstractions

1. Install from NuGet:
```
Install-Package DbConn.DbExecutor.Abstract
```
2. Then you can use `IDbExecutor` and `IDbExecutorFactory` interfaces.


### Implementations

#### Dapper

1. Install from NuGet:
```
Install-Package DbConn.DbExecutor.Dapper
```

2. After you can use `DapperDbExecutor` and `DapperDbExecutorFactory` classes. For example - business component that creates user:
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
        using (var dbExecutor = _dbExecutorFactory.Create(_appConfig.ConnectionStrings.UserDb))
        {
            try
            {
                // Some logic for creating user

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

### Dependency injection & IoC

#### Autofac
1. Install from NuGet:
```
Install-Package DbConn.DbExecutor.Dapper.Ioc.Autofac
```
2. This package have an Autofac registration module with name `DbExecutorRegistrationModule`. Just create new instanse of this class when you call `RegisterModule` method of Autofac `ContainerBuilder`:
```csharp
builder.RegisterModule(new DbExecutorRegistrationModule());
```
