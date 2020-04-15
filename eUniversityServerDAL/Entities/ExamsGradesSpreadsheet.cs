using eUniversityServer.DAL.Enums;
using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.DAL.Entities
{
    public class ExamsGradesSpreadsheet : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }

        public Guid FormOfEducationId { get; set; }
        public FormOfEducation FormOfEducation { get; set; }

        public Guid AcademicGroupId { get; set; }
        public AcademicGroup AcademicGroup { get; set; }

        public Guid AcademicDisciplineId { get; set; }
        public AcademicDiscipline AcademicDiscipline { get; set; }

        public Guid EducationProgramId { get; set; }
        public EducationProgram EducationProgram { get; set; }

        public Guid StructuralUnitId { get; set; }
        public StructuralUnit StructuralUnit { get; set; }


        public string SpreadsheetNumber { get; set; }

        public short SemesterNumber { get; set; }

        public DateTime ExamDate { get; set; }

        public ExamsSpreadsheetType ExamsSpreadsheetType { get; set; }

        public ExamsSpreadsheetAttestationType ExamsSpreadsheetAttestationType { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public ICollection<RatingForDiscipline> RatingForDisciplines { get; set; }
    }
}
