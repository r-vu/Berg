using System;
using Berg.Areas.Identity.Data;
using Berg.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Berg.Areas.Identity.IdentityHostingStartup))]
namespace Berg.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<BergUserContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("BergUserContextConnection")));

                services.AddDefaultIdentity<BergUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<BergUserContext>();
            });
        }
    }
}