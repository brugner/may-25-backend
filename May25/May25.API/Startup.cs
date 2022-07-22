using AspNetCoreRateLimit;
using AutoMapper;
using May25.API.Core.Extensions;
using May25.API.Core.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace May25.API.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));
            services.Configure<GoogleMapsOptions>(Configuration.GetSection("GoogleMaps"));
            services.Configure<GoogleFirebaseOptions>(Configuration.GetSection("GoogleFirebase"));
            services.Configure<APIKeysOptions>(Configuration.GetSection("APIKeys"));
            services.Configure<EmailOptions>(Configuration.GetSection("Email"));
            services.Configure<AppOptions>(Configuration.GetSection("App"));

            services.AddControllers();

            services.AddAppDbContext(Configuration.GetConnectionString("Main"));

            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddHttpContextAccessor();

            services.AddTransient(x => x.GetService<IHttpContextAccessor>().HttpContext.User);

            var jwtOptions = Configuration.GetSection("Jwt").Get<JwtOptions>();
            services.AddJwtAuthentication(jwtOptions);

            services.AddSwagger();

            services.AddAutoMapper(typeof(Startup));
            services.AddApplicationServices();
            services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:InstrumentationKey"]);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/api/error");
            app.UpdateDatabase(env);
            app.UseSerilogRequestLogging();

            app.UseIpRateLimiting();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "May25 API v1");
                options.DocExpansion(DocExpansion.None);
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
