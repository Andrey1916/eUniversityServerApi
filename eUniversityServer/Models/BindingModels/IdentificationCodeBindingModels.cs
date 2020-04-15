using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class IdentificationCodeBindingModel
    {
        [Range(0, long.MaxValue)]
        public long? Number { get; set; }

        [Required]
        [MaxLength(512)]
        public string IssuingAuthority { get; set; }

        public DateTime? DateOfIssue { get; set; }
    }
}
