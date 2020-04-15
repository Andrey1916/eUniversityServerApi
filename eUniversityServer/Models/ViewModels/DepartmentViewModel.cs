using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Models.ViewModels
{
    public class DepartmentViewModel
    {
        public Guid Id { get; set; }

        public Guid? StructuralUnitId { get; set; }

        public string Code { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public string FullNameEng { get; set; }

        public string Chief { get; set; }
    }

    public class DepartmentInfoViewModel : DepartmentViewModel
    {
        public string StructuralUnitName { get; set; }
    }
}
