using Autofac;

namespace ExchangeRates.DA
{
    public class DaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SessionFactory>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
