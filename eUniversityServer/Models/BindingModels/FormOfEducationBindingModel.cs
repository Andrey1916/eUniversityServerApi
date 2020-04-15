using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateFormOfEducationBindingModel
    {
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
    }

    public class UpdateFormOfEducationBindingModel : CreateFormOfEducationBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
