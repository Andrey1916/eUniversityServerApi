using eUniversityServer.DAL.Enums;
using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUniversityServer.DAL.Entities
{
    public class AcademicDiscipline : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public Guid CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; }

        public Guid LecturerId { get; set; }

        [InverseProperty("LecturerDisciplines")]
        public Teacher Lecturer { get; set; }

        public Guid? AssistantId { get; set; }

        [InverseProperty("AssistantDisciplines")]
        public Teacher Assistant { get; set; }


        public string FullName { get; set; }

        public string ShortName { get; set; }

        public SemesterType Semester { get; set; }

        public int NumberOfCredits { get; set; }

        public AttestationType Attestation { get; set; }

        public IndividualWork TypeOfIndividualWork { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public ICollection<ExamsGradesSpreadsheet> ExamsGradesSpreadsheets { get; set; }
        public ICollection<RatingForDiscipline> RatingForDisciplines { get; set; }
    }
}
