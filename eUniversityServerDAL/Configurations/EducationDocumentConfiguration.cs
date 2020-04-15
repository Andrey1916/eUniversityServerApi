using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class EducationDocumentConfiguration : IEntityTypeConfiguration<EducationDocument>
    {
        public void Configure(EntityTypeBuilder<EducationDocument> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.Series)
                   .IsRequired();

            builder.Property(c => c.Number)
                   .IsRequired();

            builder.Property(c => c.IssuingAuthority)
                   .HasMaxLength(512)
                   .IsRequired()
                   .IsUnicode();

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();


            builder.HasOne(c => c.Student)
                   .WithOne(c => c.EducationDocument)
                   .HasForeignKey<Student>(c => c.EducationDocumentId);
        }
    }
}
