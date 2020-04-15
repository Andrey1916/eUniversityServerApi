using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class EducationProgramConfiguration : IEntityTypeConfiguration<EducationProgram>
    {
        public void Configure(EntityTypeBuilder<EducationProgram> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.Language)
                   .HasMaxLength(32);

            builder.Property(c => c.Guarantor)
                   .HasMaxLength(512);

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();


            builder.HasOne(ad => ad.Specialty)
                   .WithMany(c => c.EducationPrograms)
                   .HasForeignKey(ad => ad.SpecialtyId)
                   .IsRequired();

            builder.HasOne(ad => ad.EducationLevel)
                   .WithMany(c => c.EducationPrograms)
                   .HasForeignKey(ad => ad.EducationLevelId)
                   .IsRequired();
        }
    }
}
