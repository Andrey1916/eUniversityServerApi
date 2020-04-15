using eUniversityServer.DAL.Entities;
using eUniversityServer.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.Message)
                   .IsRequired();

            builder.Property(c => c.DateTime)
                   .IsRequired();
            
            builder.Property(c => c.LogLevel)
                   .HasConversion(
                       v => (int)v,
                       v => (Level)v)
                   .IsUnicode(false);


            builder.HasOne(ad => ad.User)
                   .WithMany(c => c.Logs)
                   .HasForeignKey(ad => ad.UserId)
                   .IsRequired(false);
        }
    }
}
