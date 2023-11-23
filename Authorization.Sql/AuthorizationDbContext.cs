using Authorization.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Authorization.Sql.Configurations;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Authorization.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Authorization.Sql;

public class AuthorizationDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options)
    {

    }

    public override int SaveChanges()
    {
        SetDates();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        SetDates();

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var assembly = Assembly.GetAssembly(typeof(UserEntityConfiguration))!;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        modelBuilder.Entity<IdentityRole<Guid>>(entity => entity.ToTable(name: "Roles"));
        
        modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
            entity.ToTable(name: "UserRoles"));
      
        modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
            entity.ToTable(name: "UserClaims"));
       
        modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
            entity.ToTable("UserLogins"));
        
        modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
            entity.ToTable("UserTokens"));
        
        modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
            entity.ToTable("RoleClaims"));
    }

    private void SetDates()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e =>
                (e.Entity is IHasCreatedAt or IHasUpdatedAt) && (e.State is EntityState.Added or EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added && entityEntry.Entity is IHasCreatedAt createdEntity)
                createdEntity.CreatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Modified && entityEntry.Entity is IHasUpdatedAt modifiedEntity)
                modifiedEntity.UpdatedAt = DateTime.UtcNow;
        }
    }
}