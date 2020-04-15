using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class CreateRatingForDisciplineBindingModel
    {
        public Guid ExamsGradesSpreadsheetId { get; set; }

        public Guid AcademicDisciplineId { get; set; }

        public Guid AcademicGroupId { get; set; }

        public Guid TeacherId { get; set; }

        public Guid StudentId { get; set; }

        [Required]
        [Range(0, short.MaxValue)]
        public short Score { get; set; }

        public DateTime Date { get; set; }
    }

    public class UpdateRatingForDisciplineBindingModel : CreateRatingForDisciplineBindingModel, IBindingModel
    {
        public Guid Id { get; set; }
    }
}
