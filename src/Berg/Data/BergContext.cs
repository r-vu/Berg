using Microsoft.EntityFrameworkCore;

namespace Berg.Data {
    public class BergContext : DbContext {
        public BergContext(DbContextOptions<BergContext> options)
            : base(options) {
        }

        public DbSet<Berg.Models.Item> Item { get; set; }
    }
}
