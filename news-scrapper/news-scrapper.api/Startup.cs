using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using news_scrapper.api.Middleware;
using news_scrapper.application.Authorization;
using news_scrapper.application.Data;
using news_scrapper.application.Interfaces;
using news_scrapper.application.Repositories;
using news_scrapper.application.UnitsOfWork;
using news_scrapper.domain;
using news_scrapper.domain.DBModels;
using news_scrapper.infrastructure;
using news_scrapper.infrastructure.Authorization;
using news_scrapper.infrastructure.Data;
using news_scrapper.infrastructure.DbAccess;
using news_scrapper.infrastructure.MapperProfiles;
using news_scrapper.infrastructure.Repositories;
using news_scrapper.infrastructure.UnitsOfWork;
using System;
using System.Net;

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

            services.AddDbContext<PostgreSqlContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddHttpClient<IWebsiteService, WebsiteService>(
                opts => {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
                    opts.DefaultRequestHeaders.Accept.Clear();
                });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddTransient<IPagesScrapperService, PagesScrapperService>();
            services.AddTransient<IHtmlScrapper, HtmlScrapper>();
            services.AddTransient<IWebsiteDetailsService, WebsiteDetailsService>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IArticlesService, ArticlesService>();
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddTransient<IUserService, UserService>();

            //Repositories 
            services.AddTransient<IWebsitesRepository, WebsitesRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
            //services.AddTransient<IArticlesRepository, ArticlesRepository>();


            //UoW
            services.AddTransient<IArticlesUnitOfWork, ArticlesUnitOfWork>();
            services.AddTransient<IUsersUnitOfWork, UsersUnitOfWork>();

            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new WebsiteDetailsProfile());
                cfg.AddProfile(new ArticlesProfile());
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new RefreshTokenProfile());
            }).CreateMapper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetService<PostgreSqlContext>();
            context.MigrateDatabase();
            
            //createTestUser(context);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "news_scrapper.api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void createTestUser(PostgreSqlContext context)
        {
            var testUser = new UserDb
            {
                FirstName = "Test",
                LastName = "User",
                Username = "test",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("q")
            };

            context.Users.Add(testUser);
            context.SaveChanges();
        }
    }
}
