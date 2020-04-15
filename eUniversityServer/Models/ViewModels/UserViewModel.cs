using System;
using System.Collections.Generic;

namespace eUniversityServer.Models.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public string FirstNameEng { get; set; }

        public string LastNameEng { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public IEnumerable<RoleViewModel> Roles { get; set; }
    }

    public class SignInViewModel : UserViewModel
    {
        public TokenInfoViewModel TokenInfo { get; set; }
    }
}
