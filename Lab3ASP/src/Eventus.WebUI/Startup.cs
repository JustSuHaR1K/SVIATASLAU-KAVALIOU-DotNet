using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using BusinessLogic.Services.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Eventus.BusinessLogic.Interfaces;
using Eventus.BusinessLogic.Services;
using Eventus.DAL.Interfaces;
using Eventus.DAL.Models;
using Eventus.WebUI.Mapper;
using Eventus.DAL.Repositories;

namespace Eventus.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<EventContext>(options => options
            .UseSqlServer(Configuration.GetConnectionString("EventConnection"))
            .UseLoggerFactory(TaxiLoggerFactory(Configuration)));

            services.AddTransient<IRepository<EventDto>, EventRepository<EventDto>>();
            services.AddTransient<IRepository<ClientDto>, EventRepository<ClientDto>>();
            services.AddTransient<IRepository<MasterDto>, EventRepository<MasterDto>>();
            services.AddTransient<IRepository<OrderDto>, EventRepository<OrderDto>>();

            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IMasterService, MasterService>();
            services.AddTransient<IOrderService, OrderService>();

            services.AddTransient<EventProfile>();
            services.AddTransient<FileFromDataBase>();
            services.AddTransient<DataBaseFromFile>();

            services.AddTransient<IReader, JsonReader>();
            services.AddTransient<IWriter, JsonWriter>();

            services.AddAutoMapper(typeof(EventProfile), typeof(EventUIProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Cars/Error");
                
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Cars}/{action=Cars}");
            });
        }

        private ILoggerFactory TaxiLoggerFactory(IConfiguration configuration) => LoggerFactory.Create(builder =>
        {
            builder.AddConfiguration(configuration);
        });
    }
}