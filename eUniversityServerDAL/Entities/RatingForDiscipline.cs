using eUniversityServer.DAL.Interfaces;
using System;

namespace eUniversityServer.DAL.Entities
{
    public class RatingForDiscipline : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid ExamsGradesSpreadsheetId { get; set; }
        public ExamsGradesSpreadsheet ExamsGradesSpreadsheet { get; set; }

        public Guid AcademicDisciplineId { get; set; }
        public AcademicDiscipline AcademicDiscipline { get; set; }

        public Guid AcademicGroupId { get; set; }
        public AcademicGroup AcademicGroup { get; set; }

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; }


        public short Score { get; set; }

        public DateTime Date { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
