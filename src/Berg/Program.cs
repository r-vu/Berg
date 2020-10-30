using Berg.Data;
using Berg.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Berg {
    public class Program {
        public static void Main(string[] args) {
            IHost host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope()) {
                IServiceProvider serviceProvider = scope.ServiceProvider;

                try {
                    SeedDatabase(serviceProvider);
                    CreateDefaultUser(serviceProvider).Wait();
                } catch (Exception e) {
                    ILogger logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "An error occurred when attempting to seed the database.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });

        private static void SeedDatabase(IServiceProvider serviceProvider) {
            using (BergContext context = new BergContext(serviceProvider.GetRequiredService<DbContextOptions<BergContext>>())) {
                if (!context.Item.Any()) {
                    context.Item.AddRange(
                        new Item("Shrimp", 5.00M),
                        new Item("Canned Tuna", 2.00M),
                        new Item("Salmon", 10.00M),
                        new Item("Tilapia", 2.50M),
                        new Item("Alaska Pollock", 7.00M),
                        new Item("Catfish", 3.00M),
                        new Item("Crab", 12.00M),
                        new Item("Lobster", 20.00M),
                        new Item("Clams", 4.50M),
                        new Item("Scallops", 13.00M)
                    );
                }

                context.SaveChanges();
            }
        }

        private async static Task CreateDefaultUser(IServiceProvider serviceProvider, string defaultUserName = "bergadmin@r-vu.net") {
            UserManager<BergUser> userManager = serviceProvider.GetService<UserManager<BergUser>>();
            BergUser admin = await userManager.FindByNameAsync(defaultUserName);

            if (admin == null) {
                admin = new BergUser(defaultUserName);
                admin.EmailConfirmed = true;
                await userManager.CreateAsync(admin, "bergpassword");
            }
        }
    }
}
