using eUniversityServer.DAL.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateAcademicDisciplineBindingModel
    {
        public Guid SpecialtyId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid CurriculumId { get; set; }

        public Guid LecturerId { get; set; }

        public Guid? AssistantId { get; set; }

        [Required]
        [MaxLength(512)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(512)]
        public string ShortName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SemesterType Semester { get; set; }

        [Required]
        public int NumberOfCredits { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AttestationType Attestation { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public IndividualWork TypeOfIndividualWork { get; set; }
    }

    public class UpdateAcademicDisciplineBindingModel : CreateAcademicDisciplineBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
