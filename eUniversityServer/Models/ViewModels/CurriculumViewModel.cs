using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Models.ViewModels
{
    public class CurriculumViewModel
    {
        public Guid Id { get; set; }

        public Guid SpecialtyId { get; set; }

        public Guid StructuralUnitId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid FormOfEducationId { get; set; }

        public Guid? EducationProgramId { get; set; }

        public Guid EducationLevelId { get; set; }

        public int? YearOfAdmission { get; set; }

        public DateTime? DateOfApproval { get; set; }

        public string ScheduleOfEducationProcess { get; set; }

        public string ListOfApprovals { get; set; }

        public string OrderOfApprovals { get; set; }

        public string ProtocolOfAcademicCouncilOfUnit { get; set; }

        public string ProtocolOfAcademicCouncilOfUniversity { get; set; }

        public string SpecialtyGuarantor { get; set; }

        public string ShortName { get; set; }
    }

    public class CurriculumInfoViewModel : CurriculumViewModel
    {
        public string SpecialtyName { get; set; }

        public string StructuralUnitName { get; set; }

        public string DepartmentName { get; set; }

        public string FormOfEducation { get; set; }

        public string EducationLevel { get; set; }
    }
}
