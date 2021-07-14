namespace MessiFinder.Infrastructure
{
    using System.Linq;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            var scopeService = app.ApplicationServices.CreateScope();

            var data = scopeService.ServiceProvider.GetService<MessiFinderDbContext>();

            data?.Database.Migrate();

            SeedPlaygrounds(data);

            return app;
        }

        private static void SeedPlaygrounds(MessiFinderDbContext data)
        {
            if (data.Playgrounds.Any())
            {
                return;
            }

            data.Playgrounds.AddRange(new[]
            {
                new Playground
                {
                    Name = "Avenue", 
                    Description = "In the summer this place is number 1 to play mini football."
                },
                new Playground
                {
                    Name = "Kortove",
                    Description = "In the winter this place is number 1 to play mini football, because the players play inside."
                },
            });

            data.SaveChanges();
        }
    }
}
