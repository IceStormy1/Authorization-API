using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using Authorization.Common;

namespace Authorization.Sql.Configurations;

internal sealed class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.HasKey(r => new { r.UserId, r.RoleId });

        builder.ToTable(name: "UserRoles");

        builder.HasData(new List<IdentityUserRole<Guid>>
        {
            new()
            {
                RoleId = RoleConstants.AdminRoleId,
                UserId = Guid.Parse("f2343d16-e610-4a73-a0f0-b9f63df511e6")
            },
            new()
            {
                RoleId = RoleConstants.UserRoleId, // user
                UserId = Guid.Parse("e1f83d38-56a7-435b-94bd-fe891ed0f03a")
            }
        });
    }
}