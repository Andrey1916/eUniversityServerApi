using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class UserRolesConfiguration : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder.HasKey(t => new { t.RoleId, t.UserId });

            builder.HasOne(t => t.User)
                   .WithMany(t => t.UserRoles)
                   .HasForeignKey(t => t.UserId);

            builder.HasOne(t => t.Role)
                   .WithMany(t => t.UserRoles)
                   .HasForeignKey(t => t.RoleId);
        }
    }
}
