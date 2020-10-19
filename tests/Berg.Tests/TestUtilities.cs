using Berg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;

namespace Berg.Tests {
    public static class TestUtilities {

        public static Random RNG = new Random(123456789);

        public static DbContextOptions<BergContext> TestDbContextOptions() {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            DbContextOptionsBuilder<BergContext> builder = new DbContextOptionsBuilder<BergContext>()
                .UseInMemoryDatabase("BergTestDb")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        public static string RandomString() {
            int length = RNG.Next(3, 12);
            return Path.GetRandomFileName().Replace(".", "").Substring(0, length);
        }

    }
}
