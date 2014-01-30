using Autofac;

namespace ExchangeRates.Tests
{
    public class IntegrationDaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IntegrationSessionFactory>().AsSelf().AsImplementedInterfaces().SingleInstance();
        }
    }
}
