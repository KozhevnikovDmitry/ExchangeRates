using Autofac;

namespace ExchangeRates.BL
{
    public class BlModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExchangeRates>().AsImplementedInterfaces();
            builder.RegisterType<RateReporitory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RateService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RateClient>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
