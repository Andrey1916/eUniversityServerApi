using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.Code)
                   .HasMaxLength(16);

            builder.Property(c => c.ShortName)
                   .HasMaxLength(256);

            builder.Property(c => c.FullName)
                   .HasMaxLength(512);

            builder.Property(c => c.FullNameEng)
                   .HasMaxLength(512);

            builder.Property(c => c.Chief)
                   .HasMaxLength(512);

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();


            builder.HasOne(ad => ad.StructuralUnit)
                   .WithMany(c => c.Departments)
                   .HasForeignKey(ad => ad.StructuralUnitId);
        }
    }
}
