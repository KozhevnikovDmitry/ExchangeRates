using Autofac;

namespace ExchangeRates.BL
{
    public class BlModule : Module
    {
        /// <summary>
        /// Autafac module, that collects types from ui assembly ExchangeRate.BL
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExchangeRates>().AsImplementedInterfaces();
            builder.RegisterType<RateReporitory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RateService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RateClient>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
