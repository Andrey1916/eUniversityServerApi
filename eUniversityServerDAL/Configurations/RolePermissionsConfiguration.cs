using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eUniversityServer.DAL.Configurations
{
    class RolePermissionsConfiguration : IEntityTypeConfiguration<RolePermissions>
    {
        public void Configure(EntityTypeBuilder<RolePermissions> builder)
        {
            builder.HasKey(t => new { t.RoleId, t.PermissionId });

            builder.HasOne(t => t.Permission)
                   .WithMany(t => t.RolePermissions)
                   .HasForeignKey(t => t.PermissionId);

            builder.HasOne(t => t.Role)
                   .WithMany(t => t.RolePermissions)
                   .HasForeignKey(t => t.RoleId);
        }
    }
}
