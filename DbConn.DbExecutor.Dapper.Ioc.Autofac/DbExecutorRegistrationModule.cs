using System;
using Autofac;
using DbConn.DbExecutor.Abstract;

namespace DbConn.DbExecutor.Dapper.Ioc.Autofac
{
    public class DbExecutorRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DapperDbExecutorFactory>()
                .As<IDbExecutorFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DapperDbExecutor>()
                .As<IDbExecutor>()
                .InstancePerLifetimeScope();
        }
    }
}
