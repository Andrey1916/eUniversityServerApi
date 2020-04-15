using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateDepartmentBindingModel
    {
        public Guid? StructuralUnitId { get; set; }

        [MaxLength(16)]
        public string Code { get; set; }

        [MaxLength(256)]
        public string ShortName { get; set; }

        [Required]
        [MaxLength(512)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(512)]
        public string FullNameEng { get; set; }

        [MaxLength(512)]
        public string Chief { get; set; }
    }

    public class UpdateDepartmentBindingModel : CreateDepartmentBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
