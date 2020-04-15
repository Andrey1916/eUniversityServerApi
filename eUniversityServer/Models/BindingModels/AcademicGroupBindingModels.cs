using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateAcademicGroupBindingModel
    {
        public Guid SpecialtyId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid StructuralUnitId { get; set; }

        public Guid EducationLevelId { get; set; }

        public Guid FormOfEducationId { get; set; }

        [Required]
        [MaxLength(256)]
        public string UIN { get; set; }

        public short Grade { get; set; }

        public int Number { get; set; }

        public string Curator { get; set; }

        public string Captain { get; set; }
    }
    public class UpdateAcademicGroupBindingModel : CreateAcademicGroupBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
