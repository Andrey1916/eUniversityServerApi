using eUniversityServer.DAL.Entities;
using eUniversityServer.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eUniversityServer.DAL.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.TargetModifier)
                   .HasConversion(
                       v => (int)v,
                       v => (TargetModifier)v)
                   .IsUnicode(false);

            builder.Property(c => c.AccessModifier)
                   .HasConversion(
                       v => (int)v,
                       v => (AccessModifier)v)
                   .IsUnicode(false);
        }
    }
}
