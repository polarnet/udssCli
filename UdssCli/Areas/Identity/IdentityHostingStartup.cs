using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UdssCli.Areas.Identity.Data;
using UdssCli.Data;

[assembly: HostingStartup(typeof(UdssCli.Areas.Identity.IdentityHostingStartup))]
namespace UdssCli.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<UdssCliDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UdssCliDbUsers")));

                services.AddDefaultIdentity<UdssCliUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<UdssCliDbContext>();
            });
        }
    }
}