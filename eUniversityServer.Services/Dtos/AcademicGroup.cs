using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class AcademicGroup
    {
        public Guid Id { get; set; }

        public Guid SpecialtyId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid StructuralUnitId { get; set; }

        public Guid EducationLevelId { get; set; }

        public Guid FormOfEducationId { get; set; }

        public string UIN { get; set; }

        public string Code { get; set; }

        public short Grade { get; set; }

        public int Number { get; set; }

        public string Curator { get; set; }

        public string Captain { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class AcademicGroupInfo : AcademicGroup
    {
        public string SpecialtyName { get; set; }

        public string DepartmentName { get; set; }

        public string StructuralUnitName { get; set; }

        public string EducationLevel { get; set; }

        public string FormOfEducation { get; set; }
    }

    public class AcademicGroupValidator : AbstractValidator<AcademicGroup>
    {
        public AcademicGroupValidator()
        { }
    }
}
