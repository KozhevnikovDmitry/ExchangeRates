using Autofac;
using ExchangeRates.BL.Exceptions;
using ExchangeRates.Models;

namespace ExchangeRates.Tests
{
    public class IntegrationRoot
    {
        public IContainer Root { get; private set; }
        
        public void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<BlModule>();
            builder.RegisterModule<IntegrationDaModule>();
            builder.RegisterModule<UiModule>();
            Root = builder.Build();
        }

        public ExchangeRatesVm Resolve()
        {
            return Root.Resolve<ExchangeRatesVm>();
        }

        public IntegrationSessionFactory GetSessionFactory()
        {
            return Root.Resolve<IntegrationSessionFactory>();
        }

        public void Release()
        {
            Root.Dispose();
        }
    }
}
