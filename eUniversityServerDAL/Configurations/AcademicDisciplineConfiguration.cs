using eUniversityServer.DAL.Entities;
using eUniversityServer.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class AcademicDisciplineConfiguration : IEntityTypeConfiguration<AcademicDiscipline>
    {
        public void Configure(EntityTypeBuilder<AcademicDiscipline> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.FullName)
                   .IsRequired()
                   .HasMaxLength(512)
                   .IsUnicode();

            builder.Property(c => c.ShortName)
                   .IsRequired()
                   .HasMaxLength(256)
                   .IsUnicode();

            builder.Property(c => c.Semester)
                   .HasConversion(
                       v => (int)v,
                       v => (SemesterType)v)
                   .IsUnicode(false);

            builder.Property(c => c.NumberOfCredits)
                   .IsRequired();

            builder.Property(c => c.TypeOfIndividualWork)
                   .HasConversion(
                       v => (int)v,
                       v => (IndividualWork)v)
                   .IsUnicode(false);

            builder.Property(c => c.Attestation)
                   .HasConversion(
                       v => (int)v,
                       v => (AttestationType)v)
                   .IsUnicode(false);

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();


            builder.HasOne(ad => ad.Curriculum)
                   .WithMany(c => c.AcademicDisciplines)
                   .HasForeignKey(ad => ad.CurriculumId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(ad => ad.Specialty)
                   .WithMany(c => c.AcademicDisciplines)
                   .HasForeignKey(ad => ad.SpecialtyId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(ad => ad.Department)
                   .WithMany(c => c.AcademicDisciplines)
                   .HasForeignKey(ad => ad.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(ad => ad.Lecturer)
                   .WithMany(c => c.LecturerDisciplines)
                   .HasForeignKey(ad => ad.LecturerId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ad => ad.Assistant)
                   .WithMany(c => c.AssistantDisciplines)
                   .HasForeignKey(ad => ad.AssistantId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
