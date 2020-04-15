using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Models.ViewModels
{
    public class EducationProgramViewModel
    {
        public Guid Id { get; set; }

        public Guid SpecialtyId { get; set; }

        public Guid EducationLevelId { get; set; }

        public short? DurationOfEducation { get; set; }

        public string Language { get; set; }

        public DateTime? ApprovalYear { get; set; }

        public string Guarantor { get; set; }

        public string ShortName { get; set; }
    }

    public class EducationProgramInfoViewModel : EducationProgramViewModel
    {
        public string SpecialtyName { get; set; }

        public string EducationLevel { get; set; }
    }
}
