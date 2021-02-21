using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using WhiskyWine.BottleService.Domain.Models;
using WhiskyWine.BottleService.Domain.Interfaces;
using Microsoft.Extensions.Options;
using WhiskyWine.BottleService.Data.Repositories;
using WhiskyWine.BottleService.Data;
using WhiskyWine.BottleService.Data.Mappers;
using WhiskyWine.BottleService.Data.Models;

namespace WhiskyWine.BottleService.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BottleServiceDatabaseSettings>(this._configuration.GetSection(nameof(BottleServiceDatabaseSettings)));
            services.AddSingleton<IDatabaseSettings>(
                    c => c.GetRequiredService<IOptions<BottleServiceDatabaseSettings>>().Value);

            services.AddControllers();
            services.AddMemoryCache();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Bottle Service API",
                    Description = "WhiksyWine Bottle Service REST API v1"
                });
            });

            services.AddTransient<IBottleService, Domain.Services.BottleService>();
            services.AddSingleton<IRepository<Bottle>, BottleMongoRepository>();
            services.AddTransient<IMapper<Bottle, BottleMongoModel>, DomainToMongoModelMapper>();
            services.AddTransient<IMapper<BottleMongoModel, Bottle>, MongoToDomainModelMapper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WhiskyWine Bottle Service API v1");
            });

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
