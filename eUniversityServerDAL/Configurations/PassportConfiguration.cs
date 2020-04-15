using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class PassportConfiguration : IEntityTypeConfiguration<Passport>
    {
        public void Configure(EntityTypeBuilder<Passport> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.PassportSeries)
                   .HasMaxLength(2)
                   .IsRequired();

            builder.Property(c => c.PassportNumber)
                   .IsRequired();

            builder.Property(c => c.PassportIssuingAuthority)
                   .HasMaxLength(512)
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .IsRequired();


            builder.HasOne(c => c.Student)
                   .WithOne(c => c.Passport)
                   .HasForeignKey<Student>(c => c.PassportId);
        }
    }
}
