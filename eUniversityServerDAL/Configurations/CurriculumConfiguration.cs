using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class CurriculumConfiguration : IEntityTypeConfiguration<Curriculum>
    {
        public void Configure(EntityTypeBuilder<Curriculum> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.SpecialtyGuarantor)
                   .HasMaxLength(512);

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();


            builder.HasOne(ad => ad.Specialty)
                   .WithMany(c => c.Curricula)
                   .HasForeignKey(ad => ad.SpecialtyId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(ad => ad.StructuralUnit)
                   .WithMany(c => c.Curricula)
                   .HasForeignKey(ad => ad.StructuralUnitId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(ad => ad.Department)
                   .WithMany(c => c.Curricula)
                   .HasForeignKey(ad => ad.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(ad => ad.FormOfEducation)
                   .WithMany(c => c.Curricula)
                   .HasForeignKey(ad => ad.FormOfEducationId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(ad => ad.EducationProgram)
                   .WithMany(c => c.Curricula)
                   .HasForeignKey(ad => ad.EducationProgramId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ad => ad.EducationLevel)
                   .WithMany(c => c.Curricula)
                   .HasForeignKey(ad => ad.EducationLevelId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();
        }
    }
}
