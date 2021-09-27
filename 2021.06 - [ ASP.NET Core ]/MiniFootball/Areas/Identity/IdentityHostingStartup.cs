using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(MiniFootball.Areas.Identity.IdentityHostingStartup))]
namespace MiniFootball.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}