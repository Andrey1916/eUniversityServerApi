using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace eUniversityServer.DAL.Entities
{
    public class Curriculum : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }

        public Guid StructuralUnitId { get; set; }
        public StructuralUnit StructuralUnit { get; set; }

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public Guid FormOfEducationId { get; set; }
        public FormOfEducation FormOfEducation { get; set; }

        public Guid? EducationProgramId { get; set; }
        public EducationProgram EducationProgram { get; set; }

        public Guid EducationLevelId { get; set; }
        public EducationLevel EducationLevel { get; set; }


        public int? YearOfAdmission { get; set; }

        public DateTime? DateOfApproval { get; set; }

        public string ScheduleOfEducationProcess { get; set; }

        public string ListOfApprovals { get; set; }

        public string OrderOfApprovals { get; set; }

        public string ProtocolOfAcademicCouncilOfUnit { get; set; }

        public string ProtocolOfAcademicCouncilOfUniversity { get; set; }

        public string SpecialtyGuarantor { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public ICollection<AcademicDiscipline> AcademicDisciplines { get; set; }
    }
}
