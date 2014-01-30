using System;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using ExchangeRates.BL.Exceptions;
using ExchangeRates.DA;
using ExchangeRates.Models;

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

    [ModelBinderType(typeof(ExchangeRatesVm))]
    public class ModelBinder : DefaultModelBinder
    {
        private readonly IComponentContext _componentContext;

        public ModelBinder(IComponentContext componentContext)
        {
            this._componentContext = componentContext;
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            return this._componentContext.Resolve(modelType);
        }
    }
}