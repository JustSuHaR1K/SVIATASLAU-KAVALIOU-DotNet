using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using BusinessLogic.Services.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ninject.Modules;
using Ninject.Web.Common;
using System.IO;
using Eventus.BusinessLogic.Interfaces;
using Eventus.BusinessLogic.Services;
using Eventus.ConsoleUI.Interfaces;
using Eventus.ConsoleUI.Services;
using Eventus.DAL.Interfaces;
using Eventus.DAL.Models;
using Eventus.DAL.Repositories;


namespace Eventus.ConsoleUI.Configuration
{
    public class NinjectConfiguration : NinjectModule
    {
        public override void Load()
        {
            var config = GetConfig();

            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<EventProfile>(); });
            Bind<IMapper>().ToConstructor(c => new Mapper(mapperConfiguration)).InSingletonScope();

            var options = new DbContextOptionsBuilder<EventContext>()
                .UseSqlServer(config.GetConnectionString("EventConnection"))
                .UseLoggerFactory(EventLoggerFactory(config)).Options;
            Bind<DbContext>().To<EventContext>().InRequestScope().WithConstructorArgument("options", options);

            Bind<IRepository<EventDto>>().To<EventRepository<EventDto>>();
            Bind<IRepository<ClientDto>>().To<EventRepository<ClientDto>>();
            Bind<IRepository<MasterDto>>().To<EventRepository<MasterDto>>();
            Bind<IRepository<OrderDto>>().To<EventRepository<OrderDto>>();

            Bind<IConfiguration>().ToConstant(config);
            Bind<ILoggerFactory>().ToConstant(EventLoggerFactory(config));

            Bind<FileFromDataBase>().ToSelf();
            Bind<DataBaseFromFile>().ToSelf();
            Bind<IReader>().To<JsonReader>();
            Bind<IWriter>().To<JsonWriter>();
            Bind<EventProfile>().ToSelf();

            Bind<EventService>().ToSelf();
            Bind<MasterService>().ToSelf();
            Bind<OrderService>().ToSelf();
            Bind<RoleService>().ToSelf();

            Bind<IConsoleService<EventConsoleService>>().To<EventConsoleService>();
            Bind<IConsoleService<MasterConsoleService>>().To<MasterConsoleService>();
            Bind<IConsoleService<OrderConsoleService>>().To<OrderConsoleService>();
            Bind<IConsoleService<AdminConsoleService>>().To<AdminConsoleService>();

            Bind<IEventService>().To<EventService>();
            Bind<IMasterService>().To<MasterService>();
            Bind<IOrderService>().To<OrderService>();
        }

        private IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            return config;
        }

        private ILoggerFactory EventLoggerFactory(IConfiguration configuration) => LoggerFactory.Create(builder =>
        {
            builder.AddConfiguration(configuration);
        });
    }
}