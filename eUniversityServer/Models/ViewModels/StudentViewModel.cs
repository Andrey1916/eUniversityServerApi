using eUniversityServer.DAL.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace eUniversityServer.Models.ViewModels
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }

        public Guid? PrivilegeId { get; set; }

        public Guid AcademicGroupId { get; set; }

        public Guid? EducationProgramId { get; set; }

        public Guid FormOfEducationId { get; set; }

        public Guid EducationLevelId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SexType Sex { get; set; }

        public long StudentTicketNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Financing Financing { get; set; }

        public string AddressOfResidence { get; set; }

        public int NumberOfRecordBook { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ForeignLanguage { get; set; }

        public bool? MilitaryRegistration { get; set; }

        public string Chummery { get; set; }

        public bool AcceleratedFormOfEducation { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public string FirstNameEng { get; set; }

        public string LastNameEng { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }

    public class StudentInfoViewModel : StudentViewModel
    {
        public string AcademicGroupCode { get; set; }

        public string FormOfEducation { get; set; }

        public string EducationLevel { get; set; }
    }
}
