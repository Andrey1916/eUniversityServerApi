using System;

namespace eUniversityServer.Services.Models.Statistics
{
    public class SpecialtyStudentsDispersion
    {
        public Guid SpecialtyId { get; set; }

        public int Year { get; set; }

        public int CountOfMales { get; set; }

        public int CountOfFemales { get; set; }
    }
}
