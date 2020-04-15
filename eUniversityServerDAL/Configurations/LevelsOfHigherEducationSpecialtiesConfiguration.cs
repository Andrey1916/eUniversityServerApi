using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eUniversityServer.DAL.Configurations
{
    internal sealed class LevelsOfHigherEducationSpecialtiesConfiguration : IEntityTypeConfiguration<LevelsOfHigherEducationSpecialties>
    {
        public void Configure(EntityTypeBuilder<LevelsOfHigherEducationSpecialties> builder)
        {
            builder.HasKey(c => new { c.SpecialtyId, c.EducationLevelId });


            builder.HasOne(c => c.Specialty)
                   .WithMany(c => c.LevelsOfHigherEducationSpecialties)
                   .HasForeignKey(c => c.SpecialtyId);

            builder.HasOne(c => c.EducationLevel)
                   .WithMany(c => c.LevelsOfHigherEducationSpecialties)
                   .HasForeignKey(c => c.EducationLevelId);
        }
    }
}
