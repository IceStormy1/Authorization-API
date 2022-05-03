using Authorization.Sql.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Sql
{
    public class AuthorizationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);
            });
        }
    }
}
