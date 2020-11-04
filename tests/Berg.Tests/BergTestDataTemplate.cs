using Berg.Data;
using Berg.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Berg.Tests {
    public class BergTestDataTemplate : IDisposable {

        protected readonly DbContextOptions<BergContext> ContextOptions;
        protected IServiceProvider serviceProvider;
        private DbConnection _connection;

        private static readonly string SQL_SERVER = @"Server=(localdb)\mssqllocaldb;Database=BergTestMsSql;ConnectRetryCount=0";
        private static readonly string SQLITE = "Filename=BergTestSqlite.db";
        private static readonly string SQLITE_INMEM = "Filename=:memory:";
        private static readonly string EFCORE_INMEM = "BergTestEfCore";

        protected List<Item> ITEM_LIST = new List<Item>() {
                    new Item("itemOne", 1.00M),
                    new Item("itemTwo", 2.50M),
                    new Item("itemThree", 3.33M)
                };

        protected List<BergUser> USER_LIST = new List<BergUser>() {
            new BergUser("testuser1"),
            new BergUser("testuser2"),
            new BergUser("testuser3")
        };

        protected List<ItemReview> REVIEW_LIST;

        public enum DbType {
            SqlServerLocalDb,
            Sqlite,
            SqliteInMemory,
            EfCoreInMemory
        }

        public BergTestDataTemplate(DbType dbType) {
            ContextOptions = TestDbContextOptions(dbType);

            using (BergContext context = new BergContext(ContextOptions)) {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        public void Dispose() {
            if (_connection != null) {
                _connection.Dispose();
            }
        }

        protected async Task SeedItems() {
            using (BergContext context = new BergContext(ContextOptions)) {
                await context.Item.AddRangeAsync(ITEM_LIST);
                await context.SaveChangesAsync();
            }

        }

        protected async Task SeedUsers() {
            UserManager<BergUser> userManager = serviceProvider.GetService<UserManager<BergUser>>();
            foreach (BergUser user in USER_LIST) {
                await userManager.CreateAsync(user);
            }
        }

        protected async Task SeedReviews() {
            // Can only be called after SeedItems and SeedUsers
            // Otherwise the reviews will have nothing to link to
            // for foreign keys

            CreateReviews();
            using (BergContext context = new BergContext(ContextOptions)) {
                context.ItemReview.AttachRange(REVIEW_LIST);
                await context.SaveChangesAsync();
            }
        }

        private DbContextOptions<BergContext> TestDbContextOptions(DbType dbType) {
            ServiceCollection services = new ServiceCollection();
            DbContextOptionsBuilder<BergContext> builder = new DbContextOptionsBuilder<BergContext>();

            switch (dbType) {
                // Repeatedly creating instances of SQL Server may have
                // performance issues, consider using a fixture in the future
                case DbType.SqlServerLocalDb:
                    services.AddEntityFrameworkSqlServer();
                    services.AddDbContext<BergContext>(options => options.UseSqlServer(SQL_SERVER));
                    builder.UseSqlServer(SQL_SERVER);
                    break;
                case DbType.Sqlite:
                    services.AddEntityFrameworkSqlite();
                    services.AddDbContext<BergContext>(options => options.UseSqlite(SQLITE));
                    builder.UseSqlite(SQLITE);
                    break;
                case DbType.SqliteInMemory:
                    CreateSqliteInMemory();
                    services.AddEntityFrameworkSqlite();
                    services.AddDbContext<BergContext>(options => options.UseSqlite(_connection));
                    builder.UseSqlite(_connection);
                    break;
                case DbType.EfCoreInMemory:
                    services.AddEntityFrameworkInMemoryDatabase();
                    services.AddDbContext<BergContext>(options => options.UseInMemoryDatabase(EFCORE_INMEM));
                    builder.UseInMemoryDatabase(EFCORE_INMEM);
                    break;
                default:
                    serviceProvider = null;
                    break;
            }

            services.AddLogging();
            services.AddIdentity<BergUser, IdentityRole>()
                .AddEntityFrameworkStores<BergContext>()
                .AddDefaultTokenProviders();
            serviceProvider = services.BuildServiceProvider();
            return builder.UseInternalServiceProvider(serviceProvider).Options;
        }

        private void CreateSqliteInMemory() {
            SqliteConnection connection = new SqliteConnection(SQLITE_INMEM);
            connection.Open();
            _connection = connection;
        }

        private void CreateReviews() {
            REVIEW_LIST = new List<ItemReview>() {
            new ItemReview() {
                Owner = USER_LIST[0],
                Item = ITEM_LIST[0],
                Rating = 1.0,
                Body = "review1"
            },
            new ItemReview() {
                Owner = USER_LIST[1],
                Item = ITEM_LIST[1],
                Rating = 2.0,
                Body = "review2"
            },
            new ItemReview() {
                Owner = USER_LIST[2],
                Item = ITEM_LIST[2],
                Rating = 3.0,
                Body = "review3"
            }
    };
        }
    }
}
