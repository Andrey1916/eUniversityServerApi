using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateEducationLevelBindingModel
    {
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        
        [Required]
        [Range(0, int.MaxValue)]
        public int Number { get; set; }
    }

    public class UpdateEducationLevelBindingModel : CreateEducationLevelBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
