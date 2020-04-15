using eUniversityServer.DAL.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace eUniversityServer.Models.ViewModels
{
    public class SpecialtyStudentsDispersionViewModel
    {
        public Guid SpecialtyId { get; set; }

        public int Year { get; set; }

        public int CountOfMales { get; set; }

        public int CountOfFemales { get; set; }
    }

    public class StudentScoreForDisciplineViewModel
    {
        public Guid StudentId { get; set; }

        public Guid ScoreId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public float StudentNumber { get; set; }

        public Guid FormOfEducationId { get; set; }

        public string FormOfEducation { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SexType Sex { get; set; }

        public string Residence { get; set; }

        public float Grade { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Financing Financing { get; set; }

        public float Rating { get; set; }

        public float Score { get; set; }
    }
}
