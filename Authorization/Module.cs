using Authorization.Abstraction.Authorization;
using Authorization.Core;
using Authorization.Sql;
using Autofac;

namespace Authorization
{
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
}
