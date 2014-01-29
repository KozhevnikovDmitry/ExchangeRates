using Autofac;

namespace ExchangeRates.BL.Exceptions
{
    public class BlModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExchangeRates>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RateReporitory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RateService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RateClient>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterInstance(new AppId("9ec36de63b284d7dbc50f8a7d278ebfd"));
        }
    }
}
