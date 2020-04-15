using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.DAL.Entities
{
    public class EducationDocument : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Student Student { get; set; }

        public string Series { get; set; }

        public long? Number { get; set; }

        public string IssuingAuthority { get; set; }

        public DateTime? DateOfIssue { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
