using Autofac;
using Autofac.Integration.Mvc;
using ExchangeRates.Models;

namespace ExchangeRates
{
    public class UiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExchangeRatesVm>().AsSelf();
            builder.RegisterType<ModelBinder>().AsImplementedInterfaces();
            builder.RegisterControllers(GetType().Assembly);
            builder.RegisterModelBinders(GetType().Assembly);
            builder.RegisterModelBinderProvider();
        }
    }
}