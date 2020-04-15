using eUniversityServer.DAL.Entities;
using eUniversityServer.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.Sex)
                   .HasConversion(
                       v => (int)v,
                       v => (SexType)v)
                   .IsUnicode(false);

            builder.Property(c => c.AddressOfResidence)
                   .HasMaxLength(512);

            builder.Property(c => c.StudentTicketNumber)
                   .IsRequired();

            builder.Property(c => c.NumberOfRecordBook)
                   .IsRequired();

            builder.Property(c => c.EntryDate)
                   .IsRequired();

            builder.Property(c => c.AcceleratedFormOfEducation)
                   .IsRequired();

            builder.Property(c => c.AcceleratedFormOfEducation)
                   .HasMaxLength(512);
            
            builder.Property(c => c.ForeignLanguage)
                   .HasMaxLength(128);

            builder.Property(c => c.Chummery)
                   .HasMaxLength(512);

            builder.Property(c => c.Financing)
                   .IsRequired()
                   .HasConversion(
                       v => (int)v,
                       v => (Financing)v)
                   .IsUnicode(false);

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();


            builder.HasOne(c => c.Privilege)
                   .WithMany(c => c.Students)
                   .HasForeignKey(c => c.PrivilegeId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(c => c.AcademicGroup)
                   .WithMany(c => c.Students)
                   .HasForeignKey(c => c.AcademicGroupId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(c => c.EducationProgram)
                   .WithMany(c => c.Students)
                   .HasForeignKey(c => c.EducationProgramId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(c => c.FormOfEducation)
                   .WithMany(c => c.Students)
                   .HasForeignKey(c => c.FormOfEducationId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(c => c.EducationLevel)
                   .WithMany(c => c.Students)
                   .HasForeignKey(c => c.EducationLevelId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();
        }
    }
}