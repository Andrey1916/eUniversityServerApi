using eUniversityServer.DAL.Entities;
using eUniversityServer.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class ExamsGradesSpreadsheetConfiguration : IEntityTypeConfiguration<ExamsGradesSpreadsheet>
    {
        public void Configure(EntityTypeBuilder<ExamsGradesSpreadsheet> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasIndex(i => i.SpreadsheetNumber)
                   .IsUnique();

            builder.Property(c => c.SpreadsheetNumber)
                   .IsRequired()
                   .HasMaxLength(512);

            builder.Property(c => c.SemesterNumber)
                   .IsRequired();

            builder.Property(c => c.ExamsSpreadsheetType)
                   .HasConversion(
                       v => (int)v,
                       v => (ExamsSpreadsheetType)v)
                   .IsUnicode(false);

            builder.Property(c => c.ExamsSpreadsheetAttestationType)
                   .HasConversion(
                       v => (int)v,
                       v => (ExamsSpreadsheetAttestationType)v)
                   .IsUnicode(false);

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();


            builder.HasOne(c => c.Specialty)
                   .WithMany(c => c.ExamsGradesSpreadsheets)
                   .HasForeignKey(c => c.SpecialtyId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(c => c.FormOfEducation)
                   .WithMany(c => c.ExamsGradesSpreadsheets)
                   .HasForeignKey(c => c.FormOfEducationId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(c => c.AcademicGroup)
                   .WithMany(c => c.ExamsGradesSpreadsheets)
                   .HasForeignKey(c => c.AcademicGroupId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(c => c.AcademicDiscipline)
                .WithMany(c => c.ExamsGradesSpreadsheets)
                .HasForeignKey(c => c.AcademicDisciplineId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(c => c.EducationProgram)
                .WithMany(c => c.ExamsGradesSpreadsheets)
                .HasForeignKey(c => c.EducationProgramId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(c => c.StructuralUnit)
                .WithMany(c => c.ExamsGradesSpreadsheets)
                .HasForeignKey(c => c.StructuralUnitId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
