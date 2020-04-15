using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace eUniversityServer.DAL.Entities
{
    public class EducationProgram : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }

        public Guid EducationLevelId { get; set; }
        public EducationLevel EducationLevel { get; set; }


        public short? DurationbOfEducation { get; set; }

        public string Language { get; set; }

        public DateTime? ApprovalYear { get; set; }

        public string Guarantor { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public ICollection<Curriculum> Curricula { get; set; }
        public ICollection<ExamsGradesSpreadsheet> ExamsGradesSpreadsheets { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
