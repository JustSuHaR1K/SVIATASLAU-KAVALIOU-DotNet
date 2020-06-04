using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using BusinessLogic.Services.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Eventus.BusinessLogic.Interfaces;
using Eventus.BusinessLogic.Services;
using Eventus.DAL.Interfaces;
using Eventus.DAL.Models;
using Eventus.WebUI.Mapper;
using EventusDAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Eventus.WebUI.Identitefication;
using System.Reflection;

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
            services.AddRazorPages();
            var businessAssembly = Assembly.Load("Eventus.BusinessLogic");
            services.AddDbContext<EventContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EventConnection")).UseLoggerFactory(EventLoggerFactory(Configuration)))
                   .Scan(scan => scan.FromAssemblies(businessAssembly)
                                     .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
                                     .AsSelf()
                                     .WithScopedLifetime())
                   .AddAutoMapper(typeof(EventProfile))
                   .AddScoped(typeof(IRepository<>), typeof(EventRepository<>));
            
            
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IIdentityService))
                    .AddClasses(classes => classes.AssignableTo(typeof(IIdentityService)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IManager<>))
                    .AddClasses(classes => classes.AssignableTo(typeof(IManager<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
            //services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<EventContext>();

            services.AddTransient<IRepository<EventDto>, EventRepository<EventDto>>();
            services.AddTransient<IRepository<ClientDto>, EventRepository<ClientDto>>();
            services.AddTransient<IRepository<MasterDto>, EventRepository<MasterDto>>();
            services.AddTransient<IRepository<OrderDto>, EventRepository<OrderDto>>();
            

            services.AddSingleton<IdentityInitializer>();

            services.AddIdentity<UserDto, IdentityRole>()
              .AddEntityFrameworkStores<EventContext>()
              .AddDefaultTokenProviders();

            services.AddTransient<IMasterService, MasterService>();
            services.AddTransient<IEventService, EventService>();
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
                var identityInitializer = app?.ApplicationServices.GetService<IdentityInitializer>();
                identityInitializer.Seed().GetAwaiter().GetResult();
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Events/Error");
                
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Events}/{action=Events}");
                endpoints.MapRazorPages();
            });
        }

        private ILoggerFactory EventLoggerFactory(IConfiguration configuration) => LoggerFactory.Create(builder =>
        {
            builder.AddConfiguration(configuration);
        });
    }
}