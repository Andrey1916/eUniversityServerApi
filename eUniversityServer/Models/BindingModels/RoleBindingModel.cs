using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateRoleBindingModel
    {
        [Required]
        [MaxLength(96)]
        public string Name { get; set; }
    }

    public class UpdateRoleBindingModel : CreateRoleBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
