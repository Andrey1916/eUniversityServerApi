using eUniversityServer.DAL.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace eUniversityServer.Models.ViewModels
{
    public class ExamsGradesSpreadsheetViewModel
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

        [JsonConverter(typeof(StringEnumConverter))]
        public ExamsSpreadsheetType ExamsSpreadsheetType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ExamsSpreadsheetAttestationType ExamsSpreadsheetAttestationType { get; set; }
    }

    public class ExamsGradesSpreadsheetInfoViewModel : ExamsGradesSpreadsheetViewModel
    {
        public string AcademicGroupCode { get; set; }

        public string SpecialtyName { get; set; }

        public string NameOfFormOfEducation { get; set; }

        public string GroupCode { get; set; }

        public string DisciplineName { get; set; }

        public string StructuralUnitName { get; set; }
    }
}
