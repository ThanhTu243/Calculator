using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DataAccess;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json.Serialization;
using Service.Implementation;
using Service.Service;
using Ultilities;

namespace PublicApi
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
            services.AddSingleton<IApplicationSettings, ApplicationSettings>();
            services.AddElasticsearch(Configuration);
            services.AddScoped<IScoreService, ScoreService>();
            services.AddScoped<IElasticSearchService, ElasticSearchService>();
            services.AddScoped<ICacheHelper, CacheHelper>();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "My API - V1",
                        Version = "v1"
                    }
                 );
                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "PublicApi.xml");
                c.IncludeXmlComments(filePath);
            });
            services.AddElasticsearch(Configuration);
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();

            });

            string IsWriteOnRedis = Configuration.GetSection("AppSetting")["IsWriteOnRedis"];
            bool.TryParse(IsWriteOnRedis, out bool IsWriteOnRedisConfig);

            if (IsWriteOnRedisConfig)
            {
                services.AddDistributedMemoryCache();

                services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = Configuration.GetSection("AppSetting").GetValue<string>("RedisConfiguration");
                    options.InstanceName = "Redis";
                });

            }

            var isRedisToken = Configuration.GetSection("AppSetting").GetValue<bool>("IsRedisToken");
            if (isRedisToken)
            {
                services.AddDistributedMemoryCache();

                services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = Configuration.GetSection("AppSetting").GetValue<string>("RedisToken");
                    options.InstanceName = Configuration.GetSection("AppSetting").GetValue<string>("InstanceRedisToken");

                });

            }

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("*");
                });
                options.AddPolicy("mypolicy", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex);


            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
    }

    public static class ApplicationSettingsFactory
    {
        private static IApplicationSettings _iApplicationSetting;

        public static void InitializeApplicationSettings(IApplicationSettings iApplicationSettings)
        {

            _iApplicationSetting = iApplicationSettings;
            using (UnitOfWork uow = new UnitOfWork(iApplicationSettings.ConnectionString))
            {

                Dictionary<string, int> prizesKeyPairValue = new Dictionary<string, int>();
                Dictionary<string, int> lotteryTypesKeyPairValue = new Dictionary<string, int>();
                Dictionary<string, string> lotterySchedulesKeyPairValue = new Dictionary<string, string>();



                uow.Commit();
            }
        }
        public static IApplicationSettings GetApplicationSettings()
        {
            return _iApplicationSetting;
        }
    }
}
