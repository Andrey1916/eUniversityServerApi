using eUniversityServer.DAL.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateTeacherBindingModel
    {
        public Guid DepartmentId { get; set; }

        [MaxLength(256)]
        public string Position { get; set; }

        [MaxLength(256)]
        public string ScientificDegree { get; set; }

        [MaxLength(256)]
        public string AcademicRank { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Employment TypeOfEmployment { get; set; }


        [Required]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(256)]
        public string LastName { get; set; }

        [Required]
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

    public class UpdateTeacherBindingModel : CreateTeacherBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
