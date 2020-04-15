using eUniversityServer.DAL.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class ExamsGradesSpreadsheet
    {
        public Guid Id { get; set; }

        public Guid SpecialtyId { get; set; }

        public Guid FormOfEducationId { get; set; }

        public Guid AcademicGroupId { get; set; }

        public Guid AcademicDisciplineId { get; set; }

        public Guid EducationProgramId { get; set; }

        public Guid StructuralUnitId { get; set; }

        public string SpreadsheetNumber { get; set; }

        public short SemesterNumber { get; set; }

        public DateTime ExamDate { get; set; }

        public ExamsSpreadsheetType ExamsSpreadsheetType { get; set; }

        public ExamsSpreadsheetAttestationType ExamsSpreadsheetAttestationType { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class ExamsGradesSpreadsheetInfo : ExamsGradesSpreadsheet
    {
        public string AcademicGroupCode { get; set; }

        public string SpecialtyName { get; set; }

        public string NameOfFormOfEducation { get; set; }

        public string GroupCode { get; set; }

        public string DisciplineName { get; set; }

        public string StructuralUnitName { get; set; }
    }

    public class ExamsGradesSpreadsheetValidator : AbstractValidator<ExamsGradesSpreadsheet>
    {
        public ExamsGradesSpreadsheetValidator()
        {
            this.RuleFor(x => x.SpreadsheetNumber).NotEmpty()
                                                  .MaximumLength(512);
        }
    }
}
