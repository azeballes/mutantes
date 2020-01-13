using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mutants.Data;
using Mutants.Model;

namespace Mutants.Api
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
            services.AddScoped<Mutant>();
            services.AddScoped<IMutantRepository, MutantRepository>();
            //services.AddSingleton<Mutant>();
            //services.AddSingleton<IMutantRepository, MutantRepository>();

            services.AddDbContextPool<MutantContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MutantContext")));
            //services.AddDbContext<MutantContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("MutantContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
