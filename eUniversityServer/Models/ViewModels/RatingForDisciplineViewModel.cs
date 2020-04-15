using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Models.ViewModels
{
    public class RatingForDisciplineViewModel
    {
        public Guid Id { get; set; }

        public Guid ExamsGradesSpreadsheetId { get; set; }

        public Guid AcademicDisciplineId { get; set; }

        public Guid AcademicGroupId { get; set; }

        public Guid TeacherId { get; set; }

        public Guid StudentId { get; set; }

        public short Score { get; set; }

        public DateTime Date { get; set; }
    }

    public class RatingForDisciplineInfoViewModel : RatingForDisciplineViewModel
    {
        public string ExamsGradesSpreadsheetNumber { get; set; }

        public string AcademicGroupCode { get; set; }

        public string AcademicDisciplineName { get; set; }

        public string TeacherName { get; set; }

        public string StudentName { get; set; }
    }
}
