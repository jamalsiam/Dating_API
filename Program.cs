
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Context;
using Api.DataSeed;
using Api.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api
{
    // public class Program
    // {
        //    public static  async Task Main(string[] args)
        //     {
        //         var host = CreateWebHostBuilder(args).Build();
        //         using (var scope = host.Services.CreateScope())
        //         {
        //             var services = scope.ServiceProvider;
        //             try
        //             {
        //                 var context = services.GetRequiredService<DBContext>();
        //                 var userManager = services.GetRequiredService<UserManager<AppUser>>();
        //                 await context.Database.MigrateAsync();
        //                 await Seed.SeedUsers(userManager);
        //             } 
        //             catch (Exception ex)
        //             {
        //                 var logger = services.GetRequiredService<ILogger<Program>>();
        //                 logger.LogError(ex, "An error occured during migration");
        //             }
        //         }

        //         host.Run();
        //     }

        //     public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //         WebHost.CreateDefaultBuilder(args)
        //             .UseStartup<Startup>();
        // }

        public class Program
        {
            public static void Main(string[] args)
            {
                CreateHostBuilder(args).Build().Run();
            }

            public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
        }
    }
