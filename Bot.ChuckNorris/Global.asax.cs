using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Bot.Builder.Dialogs.Internals;

namespace Bot.ChuckNorris
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            //register the bot builder module
            builder.RegisterModule(new DialogModule());

            //register project dependencies
            builder.RegisterModule(new CustomModules());

            //Http config 
            var config = GlobalConfiguration.Configuration;

            //register controller
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //create container
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
