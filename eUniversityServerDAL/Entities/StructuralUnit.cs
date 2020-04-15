using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace eUniversityServer.DAL.Entities
{
    public class StructuralUnit : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string FullName { get; set; }

        public string FullNameEng { get; set; }

        public string ShortName { get; set; }

        public string Chief { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public ICollection<AcademicGroup> AcademicGroups { get; set; }
        public ICollection<Curriculum> Curricula { get; set; }
        public ICollection<Department> Departments { get; set; }
        public ICollection<ExamsGradesSpreadsheet> ExamsGradesSpreadsheets { get; set; }
    }
}