using System;
using Autofac;
using DbConn.DbExecutor.Abstract;
using DbConn.DbExecutor.Dapper.SqlServer;

namespace DbConn.DbExecutor.Dapper.Ioc.Autofac
{
    public class SqliteDbExecutorRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Sqlite.DapperDbExecutorFactory>()
                .As<IDbExecutorFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Sqlite.DapperDbExecutor>()
                .As<IDbExecutor>()
                .InstancePerLifetimeScope();
        }
    }
}
