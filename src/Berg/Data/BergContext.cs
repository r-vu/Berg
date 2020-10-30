using Berg.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Berg.Data {
    public class BergContext : IdentityDbContext<BergUser> {
        public BergContext(DbContextOptions<BergContext> options)
            : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.HasDefaultSchema("Berg");
            base.OnModelCreating(builder);
        }

        public DbSet<Item> Item { get; set; }
    }
}
