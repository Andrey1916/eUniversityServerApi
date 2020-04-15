using eUniversityServer.Utils.Auth;
using System;
using System.Collections.Generic;

namespace eUniversityServer.Models.ViewModels
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<PermissionViewModel> Permissions { get; set; }
    }
}
