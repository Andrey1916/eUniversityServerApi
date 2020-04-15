using eUniversityServer.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Utils.Auth
{
    public class Permission
    {
        public Permission(AccessModifier access, TargetModifier target)
        {
            TargetModifier = target;
            AccessModifier = access;
        }

        public TargetModifier TargetModifier { get; set; }

        public AccessModifier AccessModifier { get; set; }
    }
}
