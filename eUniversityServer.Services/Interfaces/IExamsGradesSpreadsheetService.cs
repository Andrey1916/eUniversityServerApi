using eUniversityServer.Services.Dtos;
using eUniversityServer.Services.Models;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Interfaces
{
    public interface IExamsGradesSpreadsheetService : IServiceMoreInfo<ExamsGradesSpreadsheet, Dtos.ExamsGradesSpreadsheetInfo>
    { }
}
