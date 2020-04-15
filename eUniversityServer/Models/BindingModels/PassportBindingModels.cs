using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class PassportBindingModel
    {
        [Required]
        [MaxLength(2)]
        public string Series { get; set; }

        [Required]
        [Range(0, long.MaxValue)]
        public long Number { get; set; }

        [Required]
        [MaxLength(512)]
        public string IssuingAuthority { get; set; }

        public DateTime? DateOfIssue { get; set; }

        public string Nationality { get; set; }

        public string RegistrationAddress { get; set; }

        public string MaritalStatus { get; set; }

        public string PlaceOfBirth { get; set; }
    }
}
