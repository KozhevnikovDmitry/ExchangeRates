using Autofac;

namespace ExchangeRates.DA
{
    /// <summary>
    /// Autafac module, that collects types from da assembly ExchangeRate.DA
    /// </summary>
    public class DaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SessionFactory>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
