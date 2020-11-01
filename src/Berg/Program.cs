using Berg.Data;
using Berg.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Berg {
    public class Program {

        private static IList<Item> ITEM_LIST;
        private static BergUser DEFAULT_USER;
        private static readonly string DEFAULT_USERNAME = "bergadmin@r-vu.net";

        // UserManager can't be in a "using" block. If it is disposed, it seems
        // to remain disposed
        private static UserManager<BergUser> USER_MANAGER;

        public static void Main(string[] args) {

            ITEM_LIST = new List<Item> {
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
            };

            IHost host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope()) {
                IServiceProvider serviceProvider = scope.ServiceProvider;
                USER_MANAGER = serviceProvider.GetService<UserManager<BergUser>>();

                try {
                    SeedItems(serviceProvider).Wait();
                    CreateDefaultUser(serviceProvider, DEFAULT_USERNAME).Wait();
                    SeedReviews(serviceProvider).Wait();
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

        private async static Task SeedItems(IServiceProvider serviceProvider) {
            using (BergContext context = new BergContext(serviceProvider.GetRequiredService<DbContextOptions<BergContext>>())) {
                if (!await context.Item.AnyAsync()) {
                    await context.Item.AddRangeAsync(ITEM_LIST);
                    await context.SaveChangesAsync();
                } else {
                    ITEM_LIST = context.Item.ToImmutableList();
                }
            }
        }

        private async static Task SeedReviews(IServiceProvider serviceProvider) {
            using (BergContext context = new BergContext(serviceProvider.GetRequiredService<DbContextOptions<BergContext>>())) {
                if (!await context.ItemReview.AnyAsync()) {
                    Item item = ITEM_LIST[0];
                    BergUser owner = DEFAULT_USER;
                    ItemReview review = new ItemReview() {
                        Owner = owner,
                        Item = item,
                        Rating = 5.0,
                        Body = "Delicious!"
                    };

                    // It is seriously important to use Attach for these kinds of objects
                    // Otherwise EF will try to create new rows for each ForeignKey
                    // the target object has if using Add/AddAsync

                    // Attach instead changes the EntityState as needed to add the object,
                    // preventing duplication errors. However, the foreign object's state
                    // must be set to modified to update its collection.
                    context.ItemReview.Attach(review);
                    context.Entry(ITEM_LIST[0]).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    Console.WriteLine("");
                }
            }
        }

        private async static Task CreateDefaultUser(IServiceProvider serviceProvider, string defaultUserName) {
            DEFAULT_USER = await USER_MANAGER.FindByNameAsync(defaultUserName);

            if (DEFAULT_USER == null) {
                DEFAULT_USER = new BergUser(defaultUserName);
                DEFAULT_USER.EmailConfirmed = true;
                await USER_MANAGER.CreateAsync(DEFAULT_USER, "bergpassword");
            }
        }
    }
}
