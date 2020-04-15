using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class AcademicGroupConfiguration : IEntityTypeConfiguration<AcademicGroup>
    {
        public void Configure(EntityTypeBuilder<AcademicGroup> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.UIN)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(c => c.Code)
                   .IsRequired()
                   .HasMaxLength(12);

            builder.Property(c => c.Grade)
                   .IsRequired();

            builder.Property(c => c.Number)
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();


            builder.HasOne(ad => ad.Department)
                   .WithMany(c => c.AcademicGroups)
                   .HasForeignKey(ad => ad.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(ad => ad.Specialty)
                   .WithMany(c => c.AcademicGroups)
                   .HasForeignKey(ad => ad.SpecialtyId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(ad => ad.StructuralUnit)
                   .WithMany(c => c.AcademicGroups)
                   .HasForeignKey(ad => ad.StructuralUnitId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(ad => ad.EducationLevel)
                   .WithMany(c => c.AcademicGroups)
                   .HasForeignKey(ad => ad.EducationLevelId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(ad => ad.FormOfEducation)
                   .WithMany(c => c.AcademicGroups)
                   .HasForeignKey(ad => ad.FormOfEducationId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();
        }
    }
}
