using Authorization.Common;
using Authorization.Entities;
using Authorization.Entities.Entities;
using Authorization.Sql.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

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
    
        modelBuilder.Entity<IdentityRole<Guid>>(entity =>
        {
            entity.ToTable(name: "Roles");
            entity.HasData(new List<IdentityRole<Guid>>
            {
                new()
                {
                    Id = RoleConstants.AdminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new()
                {
                    Id = RoleConstants.UserRoleId,
                    Name = "User",
                    NormalizedName = "USER"
                }
            });
        });

        modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
            entity.ToTable(name: "UserClaims"));
       
        modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
            entity.ToTable("UserLogins"));
        
        modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
            entity.ToTable("UserTokens"));
        
        modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
            entity.ToTable("RoleClaims"));

        var assembly = Assembly.GetAssembly(typeof(UserEntityConfiguration))!;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
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