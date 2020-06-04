using System.Reflection;
using AutoMapper;
using BusinessLogic.Services.Mapper;
using Eventus.BusinessLogic.Interfaces;
using Eventus.BusinessLogic.Services;
using Eventus.DAL.Interfaces;
using Eventus.DAL.Models;
using EventusDAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Eventus.WebAPI
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
            services.AddControllers();
            //  services.AddControllersWithViews();
            //  services.AddRazorPages();
            var businessAssembly = Assembly.Load("Eventus.BusinessLogic");
            services.AddDbContext<EventContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EventConnection")))
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

            services.AddIdentity<UserDto, IdentityRole>()
              .AddEntityFrameworkStores<EventContext>()
              .AddDefaultTokenProviders();

            services.AddTransient<IRepository<EventDto>, EventRepository<EventDto>>();
            services.AddTransient<IRepository<ClientDto>, EventRepository<ClientDto>>();
            services.AddTransient<IRepository<MasterDto>, EventRepository<MasterDto>>();
            services.AddTransient<IRepository<OrderDto>, EventRepository<OrderDto>>();

            services.AddTransient<IMasterService, MasterService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IOrderService, OrderService>();

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "Eventus.WebAPI");
                swagger.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
