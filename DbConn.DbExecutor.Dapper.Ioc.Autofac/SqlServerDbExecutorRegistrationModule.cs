using System;
using Autofac;
using DbConn.DbExecutor.Abstract;

namespace DbConn.DbExecutor.Dapper.Ioc.Autofac
{
    public class SqlServerDbExecutorRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlServer.DapperDbExecutorFactory>()
                .As<IDbExecutorFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SqlServer.DapperDbExecutor>()
                .As<IDbExecutor>()
                .InstancePerLifetimeScope();
        }
    }
}
