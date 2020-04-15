using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateStructuralUnitBindingModel
    {
        [Required]
        [MaxLength(16)]
        public string Code { get; set; }

        [Required]
        [MaxLength(512)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(512)]
        public string FullNameEng { get; set; }

        [MaxLength(512)]
        public string ShortName { get; set; }

        [MaxLength(512)]
        public string Chief { get; set; }
    }

    public class UpdateStructuralUnitBindingModel : CreateStructuralUnitBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
