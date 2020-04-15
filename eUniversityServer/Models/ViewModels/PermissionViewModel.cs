using eUniversityServer.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Models.ViewModels
{
    public class PermissionViewModel
    {
        public TargetModifier TargetModifier { get; set; }

        public AccessModifier AccessModifier { get; set; }
    }
}
