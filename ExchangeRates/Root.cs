﻿using System.Configuration;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using ExchangeRates.BL;
using ExchangeRates.DA;

namespace ExchangeRates
{
    /// <summary>
    /// Dependency root
    /// </summary>
    public static class Root
    {
        /// <summary>
        /// Collects types from all layers. Initializes mvc <see cref="DependencyResolver"/>
        /// </summary>
        public static void Collect()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<BlModule>();
            builder.RegisterModule<DaModule>();
            builder.RegisterModule<UiModule>();
            builder.RegisterInstance(new AppId(ConfigurationManager.AppSettings["openexchangerates_appid"]));
            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}