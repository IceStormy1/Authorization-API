using Authorization.Abstractions.Authorization;
using Authorization.Core.Authorization;
using Authorization.Sql;
using Autofac;

namespace Authorization.Core;

public class Module : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<AuthorizationRepository>()
            .As<IAuthorizationRepository>();

        builder
            .RegisterType<AuthorizationService>()
            .As<IAuthorizationService>();
    }
}