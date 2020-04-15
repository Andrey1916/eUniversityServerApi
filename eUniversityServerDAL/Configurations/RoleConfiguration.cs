using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.Name)
                   .IsUnique();

            builder.Property(c => c.Name)
                   .HasMaxLength(96)
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();
        }
    }
}
