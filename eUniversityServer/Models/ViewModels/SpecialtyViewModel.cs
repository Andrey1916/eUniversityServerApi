using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Models.ViewModels
{
    public class SpecialtyViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string GroupsCode { get; set; }

        public string Discipline { get; set; }
    }
}
