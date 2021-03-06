using System;
using Lab5.Data;
using Lab5.Data.SecurityTools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Lab5.Areas.Identity.IdentityHostingStartup))]
namespace Lab5.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                //services.Configure<UserManager<IdentityUser>>(option => option.PasswordHasher = new Argon2iHasher());
                
            });
        }
    }
}