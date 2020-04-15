using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace eUniversityServer.DAL.Entities
{
    public class Specialty : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string GroupsCode { get; set; }

        /// <summary>
        /// Галузь знань, Карл!!!
        /// </summary>
        public string Discipline { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public ICollection<AcademicDiscipline> AcademicDisciplines { get; set; }
        public ICollection<AcademicGroup> AcademicGroups { get; set; }
        public ICollection<Curriculum> Curricula { get; set; }
        public ICollection<EducationProgram> EducationPrograms { get; set; }
        public ICollection<ExamsGradesSpreadsheet> ExamsGradesSpreadsheets { get; set; }
        public ICollection<LevelsOfHigherEducationSpecialties> LevelsOfHigherEducationSpecialties { get; set; }
    }
}
