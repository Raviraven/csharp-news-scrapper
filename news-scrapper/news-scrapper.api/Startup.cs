using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using news_scrapper.application.Interfaces;
using news_scrapper.application.Repositories;
using news_scrapper.infrastructure;
using news_scrapper.infrastructure.Data;
using news_scrapper.infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace news_scrapper.api
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "news_scrapper.api", Version = "v1" });
            });

            services.AddHttpClient<IWebsiteService, WebsiteService>(
                opts => {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
                    opts.DefaultRequestHeaders.Accept.Clear();
                });

            services.AddTransient<IPagesScrapperService, PagesScrapperService>();
            services.AddTransient<IHtmlScrapper, HtmlScrapper>();

            //Repositories 
            services.AddTransient<IWebsitesRepository, WebsitesRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "news_scrapper.api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
