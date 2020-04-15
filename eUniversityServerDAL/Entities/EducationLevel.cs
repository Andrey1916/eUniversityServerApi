using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace eUniversityServer.DAL.Entities
{
    public class EducationLevel : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public ICollection<AcademicGroup> AcademicGroups { get; set; }
        public ICollection<Curriculum> Curricula { get; set; }
        public ICollection<EducationProgram> EducationPrograms { get; set; }
        public ICollection<LevelsOfHigherEducationSpecialties> LevelsOfHigherEducationSpecialties { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
