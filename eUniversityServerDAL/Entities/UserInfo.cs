using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.DAL.Entities
{
    public class UserInfo : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public string FirstNameEng { get; set; }

        public string LastNameEng { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }


        public User User { get; set; }

        public Student Student { get; set; }

        public Teacher Teacher { get; set; }
    }
}
