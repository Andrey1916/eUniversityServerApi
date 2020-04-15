using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreatePrivilegeBindingModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class UpdatePrivilegeBindingModel : CreatePrivilegeBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
