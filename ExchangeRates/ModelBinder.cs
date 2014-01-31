using System;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using ExchangeRates.Models;

namespace ExchangeRates
{
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