using eUniversityServer.DAL.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace eUniversityServer.Models.ViewModels
{
    public class AcademicDisciplineViewModel
    {
        public Guid Id { get; set; }

        public Guid SpecialtyId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid CurriculumId { get; set; }

        public Guid LecturerId { get; set; }

        public Guid AssistantId { get; set; }

        public string FullName { get; set; }

        public string ShortName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SemesterType Semester { get; set; }

        public int NumberOfCredits { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AttestationType Attestation { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public IndividualWork TypeOfIndividualWork { get; set; }
    }

    public class AcademicDisciplineInfoViewModel : AcademicDisciplineViewModel
    {
        public string SpecialtyName { get; set; }

        public string DepartmentName { get; set; }

        public string LecturerName { get; set; }

        public string AssistantName { get; set; }
    }
}
