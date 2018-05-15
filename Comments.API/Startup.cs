using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comments.API.Entities;
using Comments.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;

namespace Comments.API
{
    public class Startup
    {
        public static IConfigurationRoot Configuration; 

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                //.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(
                   // new XmlSerializerOutputFormatter()));
            new XmlDataContractSerializerOutputFormatter()));


#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
            // var connectionString = @"Server=(localdb)\mssqllocaldb;Database=CommentDB;Trusted_Connection=True;";

            var connectionString = Startup.Configuration["connectionString:commentDBConnectionString"];
            services.AddDbContext<CommentContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<ICommentRepository, CommentRepository>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            CommentContext commentContext)
        {
            loggerFactory.AddConsole();

            loggerFactory.AddDebug();

            //loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            else
            {
                app.UseExceptionHandler();
            }

            commentContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Comment, Models.CommentWithoutRepliesDto>();
                cfg.CreateMap<Entities.Comment, Models.CommentDto>();
                cfg.CreateMap<Entities.Reply, Models.ReplyDto>();
                cfg.CreateMap<Models.ReplyForCreationDto, Entities.Reply>();
                cfg.CreateMap<Models.ReplyForUpdateDto, Entities.Reply>();
                cfg.CreateMap<Entities.Reply, Models.ReplyForUpdateDto>();


            });

            app.UseMvc();

        }
    }
}
