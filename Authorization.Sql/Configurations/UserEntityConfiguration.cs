using System;
using Authorization.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using Authorization.Common.Enums;

namespace Authorization.Sql.Configurations;

internal sealed class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
       builder.HasKey(x => x.Id);
       builder.Property(x => x.UserName).IsRequired();
       builder.Property(x => x.NormalizedUserName).IsRequired();
       builder.Property(x => x.PasswordHash).IsRequired();
       builder.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
       builder.Property(x => x.LastName).HasMaxLength(50).IsRequired();
       builder.Property(x => x.MiddleName).HasMaxLength(50);

       builder.HasIndex(x => x.UserName).IsUnique();
       builder.HasIndex(x => new { x.LastName, x.FirstName, x.MiddleName, x.Snils });

       builder.ToTable("Users");

       builder.HasMany(x=>x.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

        builder.HasData(GetDefaultUsersData());
    }

    private static IEnumerable<UserEntity> GetDefaultUsersData()
        => new List<UserEntity>
        {
            new()
            {
                CreatedAt = DateTime.UtcNow,
                Email = "icestormyy-admin@mail.ru",
                FirstName = "Mikhail",
                LastName = "Tolmachev",
                MiddleName = "Evgenievich",
                Gender = Gender.Male,
                Id = Guid.Parse("f2343d16-e610-4a73-a0f0-b9f63df511e6"),
                PhoneNumber = "81094316687",
                PasswordHash = "AQAAAAIAAYagAAAAEKE/pdEICYAT9QuvIIo8rHAQE3cgNN5hW7JvaVnUQW8sYlzy70H1LlxOoC1xUmc59A==", // 1f23456
                UserName = "IceStormy-admin",
                NormalizedUserName = "ICESTORMY-ADMIN",
                BirthDay = new DateOnly(2001, 06, 06)
            },
            new()
            {
                CreatedAt = DateTime.UtcNow,
                Email = "icestormyy-user@mail.ru",
                FirstName = "Mikhail",
                LastName = "Tolmachev",
                MiddleName = "Evgenievich",
                Gender = Gender.Male,
                Id = Guid.Parse("e1f83d38-56a7-435b-94bd-fe891ed0f03a"),
                PhoneNumber = "89094316687",
                PasswordHash = "AQAAAAIAAYagAAAAEKE/pdEICYAT9QuvIIo8rHAQE3cgNN5hW7JvaVnUQW8sYlzy70H1LlxOoC1xUmc59A==", // 1f23456
                BirthDay = new DateOnly(2001, 06, 06),
                UserName = "IceStormy-user",
                NormalizedUserName = "ICESTORMY-USER"
            }
        };
}