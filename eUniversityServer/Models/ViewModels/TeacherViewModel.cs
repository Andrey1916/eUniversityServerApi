using eUniversityServer.DAL.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Models.ViewModels
{
    public class TeacherViewModel
    {
        public Guid Id { get; set; }

        public Guid DepartmentId { get; set; }

        public string Position { get; set; }

        public string ScientificDegree { get; set; }

        public string AcademicRank { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Employment TypeOfEmployment { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public string FirstNameEng { get; set; }

        public string LastNameEng { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }

    public class TeacherInfoViewModel : TeacherViewModel
    {
        public string DepartmentName { get; set; }
    }
}
