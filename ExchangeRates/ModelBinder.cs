using System;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using ExchangeRates.Models;
using ExchangeRates.Models.Exceptions;

namespace ExchangeRates
{
    [ModelBinderType(typeof(ExchangeRatesVm))]
    public class ModelBinder : DefaultModelBinder
    {
        private readonly IComponentContext _componentContext;

        public ModelBinder(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            try
            {
                return _componentContext.Resolve(modelType);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (DependencyResolutionException ex)
            {
                throw new FailToResolveViewModelException(ex);
            }
            catch (Exception ex)
            {
                throw new UnexpectedViewModelException(ex);
            }
        }
    }
}