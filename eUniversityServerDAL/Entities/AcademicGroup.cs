using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace eUniversityServer.DAL.Entities
{
    public class AcademicGroup : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public Guid StructuralUnitId { get; set; }
        public StructuralUnit StructuralUnit { get; set; }

        public Guid EducationLevelId { get; set; }
        public EducationLevel EducationLevel { get; set; }

        public Guid FormOfEducationId { get; set; }
        public FormOfEducation FormOfEducation { get; set; }


        public string UIN { get; set; }

        public string Code { get; set; }

        public short Grade { get; set; }

        public int Number { get; set; }

        public string Curator { get; set; }

        public string Captain { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public ICollection<ExamsGradesSpreadsheet> ExamsGradesSpreadsheets { get; set; }
        public ICollection<RatingForDiscipline> RatingForDisciplines { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
