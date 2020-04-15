using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Models.ViewModels
{
    public class IdentificationCodeViewModel
    {
        public long? IdentificationCodeNumber { get; set; }

        public string IdentificationCodeIssuingAuthority { get; set; }

        public DateTime? IdentificationCodeDateOfIssue { get; set; }
    }
}
