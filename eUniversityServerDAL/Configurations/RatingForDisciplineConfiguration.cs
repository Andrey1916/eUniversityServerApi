using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class RatingForDisciplineConfiguration : IEntityTypeConfiguration<RatingForDiscipline>
    {
        public void Configure(EntityTypeBuilder<RatingForDiscipline> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.Score)
                   .IsRequired();

            builder.Property(c => c.Date)
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();


            builder.HasOne(c => c.ExamsGradesSpreadsheet)
                   .WithMany(c => c.RatingForDisciplines)
                   .HasForeignKey(c => c.ExamsGradesSpreadsheetId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(c => c.AcademicDiscipline)
                   .WithMany(c => c.RatingForDisciplines)
                   .HasForeignKey(c => c.AcademicDisciplineId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(c => c.AcademicGroup)
                   .WithMany(c => c.RatingForDisciplines)
                   .HasForeignKey(c => c.AcademicGroupId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(c => c.Teacher)
                   .WithMany(c => c.RatingForDisciplines)
                   .HasForeignKey(c => c.TeacherId);

            builder.HasOne(c => c.Student)
                   .WithMany(c => c.RatingForDisciplines)
                   .HasForeignKey(c => c.StudentId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();
        }
    }
}
