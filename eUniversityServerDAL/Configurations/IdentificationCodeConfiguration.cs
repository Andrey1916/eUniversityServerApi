using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class IdentificationCodeConfiguration : IEntityTypeConfiguration<IdentificationCode>
    {
        public void Configure(EntityTypeBuilder<IdentificationCode> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.IdentificationCodeDateOfIssue)
                   .IsRequired();

            builder.Property(c => c.IdentificationCodeNumber)
                   .IsRequired();

            builder.Property(c => c.IdentificationCodeIssuingAuthority)
                   .HasMaxLength(512)
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();


            builder.HasOne(c => c.Student)
                   .WithOne(c => c.IdentificationCode)
                   .HasForeignKey<Student>(c => c.IdentificationCodeId);
        }
    }
}
