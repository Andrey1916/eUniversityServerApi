using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateSpecialtyBindingModel
    {
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }

        [Required]
        [MaxLength(32)]
        public string Code { get; set; }

        [Required]
        [MaxLength(16)]
        public string GroupsCode { get; set; }

        [MaxLength(512)]
        public string Discipline { get; set; }
    }
    public class UpdateSpecialtyBindingModel : CreateSpecialtyBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
