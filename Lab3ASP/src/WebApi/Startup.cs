using AutoMapper;
using BusinessLogic.Services.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Eventus.BusinessLogic.Interfaces;
using Eventus.DAL.Interfaces;
using Eventus.DAL.Models;
using EventusDAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace WebApi
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
            //(c =>
            //{
            //    c.RouteTemplate = "/swagger/v1/swagger.json";
            //})

            app.UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi");
                swagger.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //private ILoggerFactory EventLoggerFactory(IConfiguration configuration) => LoggerFactory.Create(builder =>
        //{
        //    builder.AddConfiguration(configuration);
        //});
    }
}