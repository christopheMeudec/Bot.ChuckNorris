using Autofac;
using Bot.ChuckNorris.BusinessServices;
using Bot.ChuckNorris.DataAccess;
using System.Configuration;

namespace Bot.ChuckNorris.Scraper
{
    class Program
    {
        private static IContainer Container { get; set; }

        private static void RegisterTypes()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ChuckNorrisService>()
                .As<IChuckNorrisService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ScraperInfoService>()
                .As<IScraperInfoService>()
                .AsImplementedInterfaces()
                .SingleInstance();


            builder.RegisterType<ChuckNorrisRepository>()
                .As<IChuckNorrisRepository>()
                .AsImplementedInterfaces()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(string), ConfigurationManager.AppSettings["jsonPath"]));

            builder.RegisterType<ScraperInfoRepository>()
                .As<IScraperInfoRepository>()
                .AsImplementedInterfaces()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(string), ConfigurationManager.AppSettings["jsonPath"]));

            builder.RegisterType<ScraperExecution>()
                .As<IScraperExecution>()
                .AsImplementedInterfaces()
                .SingleInstance();

            Container = builder.Build();
        }

        static void Main(string[] args)
        {
            RegisterTypes();

            using (var scope = Container.BeginLifetimeScope())
            {
                var batchService = scope.Resolve<IScraperExecution>();
                batchService.Run();
            }
        }
    }
}
