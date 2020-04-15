using eUniversityServer.DAL.Entities;
using eUniversityServer.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.Position)
                   .HasMaxLength(256);

            builder.Property(c => c.ScientificDegree)
                   .HasMaxLength(256);

            builder.Property(c => c.AcademicRank)
                   .HasMaxLength(256);

            builder.Property(c => c.TypeOfEmployment)
                   .HasConversion(
                       v => (int)v,
                       v => (Employment)v)
                   .IsUnicode(false);

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();


            builder.HasOne(t => t.Department)
                   .WithMany(t => t.Teachers)
                   .HasForeignKey(t => t.DepartmentId)
                   .IsRequired();
        }
    }
}