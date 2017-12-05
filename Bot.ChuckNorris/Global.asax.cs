using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Configuration;
using Autofac;
using Autofac.Integration.WebApi;
using Bot.ChuckNorris.BusinessServices;
using Bot.ChuckNorris.DataAccess;
using Bot.ChuckNorris.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;

namespace Bot.ChuckNorris
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Conversation.UpdateContainer(builder =>
            {
                builder.RegisterType<RootDialog>()
                    .As<IDialog<object>>().InstancePerDependency();

                builder.RegisterType<ChuckNorrisService>()
                    .Keyed<IChuckNorrisService>(FiberModule.Key_DoNotSerialize)
                    .AsImplementedInterfaces()
                    .SingleInstance();

                builder.RegisterType<ChuckNorrisRepository>()
                    .Keyed<IChuckNorrisRepository>(FiberModule.Key_DoNotSerialize)
                    .AsImplementedInterfaces()
                    .SingleInstance()
                    .WithParameter(new TypedParameter(typeof(string), ConfigurationManager.AppSettings["jsonPath"]));
            });

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
