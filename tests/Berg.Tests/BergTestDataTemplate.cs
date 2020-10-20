using Berg.Data;
using Berg.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Berg.Tests {
    public class BergTestDataTemplate : IDisposable {

        protected readonly DbContextOptions<BergContext> ContextOptions;
        private readonly DbConnection Connection;

        protected static readonly List<Item> ITEM_LIST = new List<Item>() {
                    new Item("itemOne", 1.00M),
                    new Item("itemTwo", 2.50M),
                    new Item("itemThree", 3.33M)
                };

        public enum DbType {
            SqlServerLocalDb,
            Sqlite,
            SqliteInMemory,
            EfCoreInMemory
        }

        public BergTestDataTemplate(DbType dbType) {
            ContextOptions = TestDbContextOptions(dbType);
            Connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
            SeedDatabase();
        }

        public void Dispose() {
            if (Connection != null) {
                Connection.Dispose();
            }
        }

        private void SeedDatabase() {
            using (BergContext context = new BergContext(ContextOptions)) {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Item.AddRange(ITEM_LIST);
                context.SaveChanges();
            }
        }

        private static DbContextOptions<BergContext> TestDbContextOptions(DbType dbType) {
            ServiceProvider serviceProvider;
            DbContextOptionsBuilder<BergContext> builder = new DbContextOptionsBuilder<BergContext>();

            switch (dbType) {
                // Repeatedly creating instances of SQL Server may have
                // performance issues, consider using a fixture in the future
                case DbType.SqlServerLocalDb:
                    serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkSqlServer()
                        .BuildServiceProvider();
                    builder = builder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BergTestMsSql;ConnectRetryCount=0");
                    break;
                case DbType.Sqlite:
                    serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkSqlite()
                        .BuildServiceProvider();
                    builder = builder.UseSqlite("Filename=BergTestSqlite.db");
                    break;
                case DbType.SqliteInMemory:
                    serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkSqlite()
                        .BuildServiceProvider();
                    builder = builder.UseSqlite(CreateSqliteInMemory());
                    break;
                case DbType.EfCoreInMemory:
                    serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();
                    builder = builder.UseInMemoryDatabase("BergTestEfCore");
                    break;
                default:
                    serviceProvider = null;
                    break;
            }

            return builder.UseInternalServiceProvider(serviceProvider).Options;
        }

        private static DbConnection CreateSqliteInMemory() {
            SqliteConnection connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }
    }
}
