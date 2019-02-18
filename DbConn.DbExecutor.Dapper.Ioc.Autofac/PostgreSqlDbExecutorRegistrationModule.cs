using System;
using Autofac;
using DbConn.DbExecutor.Abstract;
using DbConn.DbExecutor.Dapper.SqlServer;

namespace DbConn.DbExecutor.Dapper.Ioc.Autofac
{
    public class PostgreSqlDbExecutorRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Npgsql.DapperDbExecutorFactory>()
                .As<IDbExecutorFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Npgsql.DapperDbExecutor>()
                .As<IDbExecutor>()
                .InstancePerLifetimeScope();
        }
    }
}
