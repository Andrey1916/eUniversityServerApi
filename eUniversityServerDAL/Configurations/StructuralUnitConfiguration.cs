using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class StructuralUnitConfiguration : IEntityTypeConfiguration<StructuralUnit>
    {
        public void Configure(EntityTypeBuilder<StructuralUnit> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.Code)
                   .HasMaxLength(16);

            builder.Property(c => c.FullName)
                   .IsRequired()
                   .HasMaxLength(512);

            builder.Property(c => c.FullNameEng)
                   .IsRequired()
                   .HasMaxLength(512);

            builder.Property(c => c.ShortName)
                   .HasMaxLength(256);

            builder.Property(c => c.Chief)
                   .HasMaxLength(512);

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();
        }
    }
}
