using System.Configuration;
using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Bot.ChuckNorris.BusinessServices;
using Bot.ChuckNorris.Dialogs;
using Bot.ChuckNorris.DataAccess;

namespace Bot.ChuckNorris
{
    public class CustomModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

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
        }
    }
}