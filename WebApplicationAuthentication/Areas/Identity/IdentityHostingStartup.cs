using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(WebApplicationAuthentication.Areas.Identity.IdentityHostingStartup))]
namespace WebApplicationAuthentication.Areas.Identity
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