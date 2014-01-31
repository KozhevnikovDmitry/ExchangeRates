using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using ExchangeRates.BL;
using ExchangeRates.DA;

namespace ExchangeRates
{
    public class Root
    {
        public static void Collect()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<BlModule>();
            builder.RegisterModule<DaModule>();
            builder.RegisterModule<UiModule>();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}