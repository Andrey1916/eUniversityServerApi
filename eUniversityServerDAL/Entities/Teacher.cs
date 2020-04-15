using eUniversityServer.DAL.Enums;
using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace eUniversityServer.DAL.Entities
{
    public class Teacher : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public Guid UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }

        public string Position { get; set; }

        public string ScientificDegree { get; set; }

        public string AcademicRank { get; set; }

        public Employment TypeOfEmployment { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public ICollection<RatingForDiscipline> RatingForDisciplines { get; set; }
        public ICollection<AcademicDiscipline> LecturerDisciplines { get; set; }
        public ICollection<AcademicDiscipline> AssistantDisciplines { get; set; }
    }
}
