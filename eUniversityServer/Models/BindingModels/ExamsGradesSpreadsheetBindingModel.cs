using eUniversityServer.DAL.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateExamsGradesSpreadsheetBindingModel
    {
        public Guid SpecialtyId { get; set; }

        public Guid FormOfEducationId { get; set; }

        public Guid AcademicGroupId { get; set; }

        public Guid AcademicDisciplineId { get; set; }

        public Guid EducationProgramId { get; set; }

        public Guid StructuralUnitId { get; set; }

        [Required]
        [MaxLength(512)]
        public string SpreadsheetNumber { get; set; }

        [Required]
        [Range(0, short.MaxValue)]
        public short SemesterNumber { get; set; }

        [Required]
        public DateTime ExamDate { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public ExamsSpreadsheetType ExamsSpreadsheetType { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public ExamsSpreadsheetAttestationType ExamsSpreadsheetAttestationType { get; set; }
    }

    public class UpdateExamsGradesSpreadsheetBindingModel : CreateExamsGradesSpreadsheetBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
