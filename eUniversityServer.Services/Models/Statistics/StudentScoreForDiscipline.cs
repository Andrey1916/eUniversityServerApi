using eUniversityServer.DAL.Enums;
using System;

namespace eUniversityServer.Services.Models.Statistics
{
    public class StudentScoreForDiscipline
    {
        public Guid StudentId { get; set; }

        public Guid ScoreId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public float StudentNumber { get; set; }

        public Guid FormOfEducationId { get; set; }

        public string FormOfEducation { get; set; }

        public SexType Sex { get; set; }

        public string Residence { get; set; }

        public float Grade { get; set; }

        public Financing Financing { get; set; }

        public float Rating { get; set; }

        public float Score { get; set; }
    }
}
