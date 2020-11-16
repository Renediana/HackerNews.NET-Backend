using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackerNewsApi.Controllers;
using LettuceEncrypt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HackerNewsApi
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
            services.AddCors(options => options.AddDefaultPolicy(builder => builder.WithOrigins("http://bonenga.ddns.net:4200").AllowCredentials().AllowAnyHeader().AllowAnyMethod()));
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);

            services.AddHttpClient<NewsService>();
            services.AddHttpContextAccessor();
            //services.AddSingleton<NewsService>();

            services.AddSingleton<INewsService, CachingNewsService>();
            services.AddSingleton<IVotesService, VotesService>();
            /*services
                .AddLettuceEncrypt()
                .PersistDataToDirectory(new DirectoryInfo("/home/rene/Documents/HackerNews.NET-Backend"), "Password123");*/



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                var hasCookie = context.Request.Cookies.ContainsKey("voterID");
                if (!hasCookie)
                {
                    context.Response.OnStarting(() =>
                    {
                        context.Response.Cookies.Append("voterID", Guid.NewGuid().ToString());
                        return Task.CompletedTask;
                    });
                }
                await next.Invoke();

            });

            app.UseRouting();
            app.UseCors();

            //app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
