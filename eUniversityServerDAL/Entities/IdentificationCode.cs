using eUniversityServer.DAL.Interfaces;
using System;

namespace eUniversityServer.DAL.Entities
{
    public class IdentificationCode : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public long? IdentificationCodeNumber { get; set; }

        public string IdentificationCodeIssuingAuthority { get; set; }

        public DateTime? IdentificationCodeDateOfIssue { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public Student Student { get; set; }
    }
}
