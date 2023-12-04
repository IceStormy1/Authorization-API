using Authorization.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
    }
}