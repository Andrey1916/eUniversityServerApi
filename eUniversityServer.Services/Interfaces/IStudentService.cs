using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Interfaces
{
    public interface IStudentService : IServiceMoreInfo<Dtos.Student, Dtos.StudentInfo>
    {
        Task<Dtos.Passport> GetPassportAsync(Guid studentId);
        Task SetPassportAsync(Guid studentId, Dtos.Passport dto);
        Task RemovePassportAsync(Guid studentId);

        Task<Dtos.IdentificationCode> GetIdentificationCodeAsync(Guid studentId);
        Task SetIdentificationCodeAsync(Guid studentId, Dtos.IdentificationCode dto);
        Task RemoveIdentificationCodeAsync(Guid studentId);

        Task<Dtos.EducationDocument> GetEducationDocumentAsync(Guid studentId);
        Task SetEducationDocumentAsync(Guid studentId, Dtos.EducationDocument dto);
        Task RemoveEducationDocumentAsync(Guid studentId);


        Task<IEnumerable<Dtos.Student>> GetStudentBySpecialty(Guid specialtyId);
    }
}
