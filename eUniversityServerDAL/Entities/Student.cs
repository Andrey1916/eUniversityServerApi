using eUniversityServer.DAL.Enums;
using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace eUniversityServer.DAL.Entities
{
    public class Student : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }

        public Guid? PrivilegeId { get; set; }
        public Privilege Privilege { get; set; }

        public Guid AcademicGroupId { get; set; }
        public AcademicGroup AcademicGroup { get; set; }

        public Guid? EducationProgramId { get; set; }
        public EducationProgram EducationProgram { get; set; }

        public Guid FormOfEducationId { get; set; }
        public FormOfEducation FormOfEducation { get; set; }

        public Guid EducationLevelId { get; set; }
        public EducationLevel EducationLevel { get; set; }

        public Guid? IdentificationCodeId { get; set; }
        public IdentificationCode IdentificationCode { get; set; }

        public Guid? PassportId { get; set; }
        public Passport Passport { get; set; }

        public Guid? EducationDocumentId { get; set; }
        public EducationDocument EducationDocument { get; set; }

        public SexType Sex { get; set; }

        public long StudentTicketNumber { get; set; }

        public Financing Financing { get; set; }

        public string AddressOfResidence { get; set; }

        public int NumberOfRecordBook { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ForeignLanguage { get; set; }

        public bool? MilitaryRegistration { get; set; }

        public string Chummery { get; set; }

        public bool AcceleratedFormOfEducation { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public ICollection<RatingForDiscipline> RatingForDisciplines { get; set; }
    }
}
