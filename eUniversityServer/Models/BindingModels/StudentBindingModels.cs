using eUniversityServer.DAL.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateStudentBindingModel
    {
        public Guid? PrivilegeId { get; set; }

        public Guid AcademicGroupId { get; set; }

        public Guid? EducationProgramId { get; set; }

        public Guid FormOfEducationId { get; set; }

        public Guid EducationLevelId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SexType Sex { get; set; }

        [Range(0, long.MaxValue)]
        public long StudentTicketNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Financing Financing { get; set; }

        [MaxLength(512)]
        public string AddressOfResidence { get; set; }

        [Range(0, int.MaxValue)]
        public int NumberOfRecordBook { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(256)]
        public string ForeignLanguage { get; set; }

        public bool? MilitaryRegistration { get; set; }

        [MaxLength(512)]
        public string Chummery { get; set; }

        public bool AcceleratedFormOfEducation { get; set; }


        [Required]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(256)]
        public string LastName { get; set; }

        [MaxLength(256)]
        public string Patronymic { get; set; }

        [Required]
        [MaxLength(256)]
        public string FirstNameEng { get; set; }

        [Required]
        [MaxLength(256)]
        public string LastNameEng { get; set; }

        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(16)]
        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }

    public class UpdateStudentBindingModel : CreateStudentBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
