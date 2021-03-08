using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WhiskyWine.AccountService.Data.Repositories;
using WhiskyWine.AccountService.Domain.Interfaces;
using WhiskyWine.AccountService.Domain.Models;
using WhiskyWine.AccountService.Domain.Services;

namespace WhiskyWine.AccountService.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMemoryCache();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WhiskyWine AccountService API",
                    Description = "WhiskyWine AccountService API",
                    Contact = new OpenApiContact
                    {
                        Name = "Jack Waters", Email = "jackwaters2@icloud.com"
                    },
                });
            });

            services.AddTransient<IAccountService, Domain.Services.AccountService>();
            services.AddTransient<IRepository<Account>, AccountRepository>();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WhiskyWine AccountService API");
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
