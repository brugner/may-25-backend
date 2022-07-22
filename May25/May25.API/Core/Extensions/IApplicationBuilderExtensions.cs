using May25.API.Core.Contracts.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace May25.API.Core.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void UpdateDatabase(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetService<IDbManagerService>();

            dbInitializer.Migrate();

            if (env.IsDevelopment())
            {
                dbInitializer.Seed();
            }
        }
    }
}
