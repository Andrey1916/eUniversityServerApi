using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace eUniversityServer.DAL.Entities
{
    public class FormOfEducation : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public ICollection<AcademicGroup> AcademicGroups { get; set; }
        public ICollection<Curriculum> Curricula { get; set; }
        public ICollection<ExamsGradesSpreadsheet> ExamsGradesSpreadsheets { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
