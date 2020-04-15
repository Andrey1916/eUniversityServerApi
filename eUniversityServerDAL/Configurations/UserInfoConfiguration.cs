using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                   .IsRequired()
                   .HasMaxLength(256)
                   .IsUnicode();

            builder.Property(c => c.LastName)
                   .IsRequired()
                   .HasMaxLength(256)
                   .IsUnicode();

            builder.Property(c => c.Patronymic)
                   .IsRequired()
                   .HasMaxLength(256)
                   .IsUnicode();

            builder.Property(c => c.FirstNameEng)
                   .IsRequired()
                   .HasMaxLength(256)
                   .IsUnicode();

            builder.Property(c => c.LastNameEng)
                   .IsRequired()
                   .HasMaxLength(256)
                   .IsUnicode();

            builder.Property(c => c.PhoneNumber)
                   .HasMaxLength(16);

            builder.Property(c => c.Email)
                   .HasMaxLength(512);


            builder.HasOne(c => c.Student)
                   .WithOne(c => c.UserInfo)
                   .HasForeignKey<Student>(c => c.UserInfoId);

            builder.HasOne(c => c.Teacher)
                   .WithOne(c => c.UserInfo)
                   .HasForeignKey<Teacher>(c => c.UserInfoId);

            builder.HasOne(c => c.User)
                   .WithOne(c => c.UserInfo)
                   .HasForeignKey<User>(c => c.UserInfoId);
        }
    }
}
