using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateEducationProgramBindingModel
    {
        public Guid SpecialtyId { get; set; }

        public Guid EducationLevelId { get; set; }

        [Range(0, short.MaxValue)]
        public short? DurationOfEducation { get; set; }

        [MaxLength(32)]
        public string Language { get; set; }

        public DateTime? ApprovalYear { get; set; }

        [MaxLength(512)]
        public string Guarantor { get; set; }
    }
    public class UpdateEducationProgramBindingModel : CreateEducationProgramBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
