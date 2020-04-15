using eUniversityServer.DAL.Interfaces;
using System;

namespace eUniversityServer.DAL.Entities
{
    public class Passport : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Student Student { get; set; }

        public string PassportSeries { get; set; }

        public long? PassportNumber { get; set; }

        public string PassportIssuingAuthority { get; set; }

        public DateTime? PassportDateOfIssue { get; set; }

        public string Nationality { get; set; }

        public string RegistrationAddress { get; set; }

        public string MaritalStatus { get; set; }

        public string PlaceOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
