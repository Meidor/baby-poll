using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BabyPoll
{
    public static class DataSeeder
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetService<BabyPollContext>();

            if (!context.Database.EnsureCreated())
            {
                context.Database.Migrate();
            }
        }
    }
}