using System;

namespace eUniversityServer.DAL.Entities
{
    public class LevelsOfHigherEducationSpecialties
    {
        public Guid SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }

        public Guid EducationLevelId { get; set; }
        public EducationLevel EducationLevel { get; set; }
    }
}
