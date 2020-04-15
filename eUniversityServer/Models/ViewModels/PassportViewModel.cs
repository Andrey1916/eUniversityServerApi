using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Models.ViewModels
{
    public class PassportViewModel
    {
        public string PassportSeries { get; set; }

        public long? PassportNumber { get; set; }

        public string PassportIssuingAuthority { get; set; }

        public DateTime? PassportDateOfIssue { get; set; }

        public string Nationality { get; set; }

        public string RegistrationAddress { get; set; }

        public string MaritalStatus { get; set; }

        public string PlaceOfBirth { get; set; }
    }
}
