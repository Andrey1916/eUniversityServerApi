using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class RatingForDiscipline
    {
        public Guid Id { get; set; }

        public Guid ExamsGradesSpreadsheetId { get; set; }

        public Guid AcademicDisciplineId { get; set; }

        public Guid AcademicGroupId { get; set; }

        public Guid TeacherId { get; set; }

        public Guid StudentId { get; set; }

        public short Score { get; set; }

        public DateTime Date { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class RatingForDisciplineInfo : RatingForDiscipline
    {
        public string ExamsGradesSpreadsheetNumber { get; set; }

        public string AcademicGroupCode { get; set; }

        public string AcademicDisciplineName { get; set; }

        public string TeacherName { get; set; }

        public string StudentName { get; set; }
    }

    public class RatingForDisciplineValidator : AbstractValidator<RatingForDiscipline>
    {
        public RatingForDisciplineValidator()
        { }
    }
}
