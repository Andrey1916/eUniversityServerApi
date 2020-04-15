using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Models.ViewModels
{
    public class EducationDocumentViewModel
    {
        public string Series { get; set; }

        public long? Number { get; set; }

        public string IssuingAuthority { get; set; }

        public DateTime? DateOfIssue { get; set; }
    }
}
