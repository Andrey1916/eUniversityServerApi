using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Models.ViewModels
{
    public class AcademicGroupViewModel
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
    }

    public class AcademicGroupInfoViewModel : AcademicGroupViewModel
    {
        public string SpecialtyName { get; set; }

        public string DepartmentName { get; set; }

        public string StructuralUnitName { get; set; }

        public string EducationLevel { get; set; }

        public string FormOfEducation { get; set; }
    }
}
