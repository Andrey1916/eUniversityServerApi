using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateCurriculumBindingModel
    {
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

        [MaxLength(512)]
        public string SpecialtyGuarantor { get; set; }
    }
    public class UpdateCurriculumBindingModel : CreateCurriculumBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
