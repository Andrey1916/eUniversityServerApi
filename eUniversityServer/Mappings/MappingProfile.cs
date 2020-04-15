using AutoMapper;
using eUniversityServer.Services.Models.Statistics;
using eUniversityServer.Utils.Auth;
using System.Linq;

namespace eUniversityServer.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DAL.Entities.Role, Services.Dtos.Role>().ReverseMap();
            CreateMap<DAL.Entities.User, Services.Dtos.User>()
                .ForMember(d => d.Roles, s => s.MapFrom(i => i.UserRoles.Select(x => x.Role))).ReverseMap();

            // view models

            // roles
            CreateMap<Services.Dtos.Role, Models.ViewModels.RoleViewModel>();
            //users
            CreateMap<Services.Dtos.User, Models.ViewModels.UserViewModel>();
            CreateMap<Services.Dtos.TokenInfo, Models.ViewModels.TokenInfoViewModel>();
            CreateMap<Services.Dtos.UserWithToken, Models.ViewModels.SignInViewModel>();
            // students
            CreateMap<Services.Dtos.Student, Models.ViewModels.StudentViewModel>();
            CreateMap<Services.Dtos.Passport, Models.ViewModels.PassportViewModel>();
            CreateMap<Services.Dtos.IdentificationCode, Models.ViewModels.IdentificationCodeViewModel>();
            CreateMap<Services.Dtos.EducationDocument, Models.ViewModels.EducationDocumentViewModel>();
            // academic disciplines
            CreateMap<Services.Dtos.AcademicDiscipline, Models.ViewModels.AcademicDisciplineViewModel>();
            // academic groups
            CreateMap<Services.Dtos.AcademicGroup, Models.ViewModels.AcademicGroupViewModel>();
            // curricula
            CreateMap<Services.Dtos.Curriculum, Models.ViewModels.CurriculumViewModel>();
            // departments
            CreateMap<Services.Dtos.Department, Models.ViewModels.DepartmentViewModel>();
            // education levels
            CreateMap<Services.Dtos.EducationLevel, Models.ViewModels.EducationLevelViewModel>();
            // education programs
            CreateMap<Services.Dtos.EducationProgram, Models.ViewModels.EducationProgramViewModel>();
            // exams grades spreadsheets
            CreateMap<Services.Dtos.ExamsGradesSpreadsheet, Models.ViewModels.ExamsGradesSpreadsheetViewModel>();
            // forms of education
            CreateMap<Services.Dtos.FormOfEducation, Models.ViewModels.FormOfEducationViewModel>();
            // permission
            CreateMap<Services.Dtos.Permission, Models.ViewModels.PermissionViewModel>();
            // privileges
            CreateMap<Services.Dtos.Privilege, Models.ViewModels.PrivilegeViewModel>();
            // rating for disciplines
            CreateMap<Services.Dtos.RatingForDiscipline, Models.ViewModels.RatingForDisciplineViewModel>();
            // specialties
            CreateMap<Services.Dtos.Specialty, Models.ViewModels.SpecialtyViewModel>();
            // structural units
            CreateMap<Services.Dtos.StructuralUnit, Models.ViewModels.StructuralUnitViewModel>();
            // teachers
            CreateMap<Services.Dtos.Teacher, Models.ViewModels.TeacherViewModel>();
            // logs
            CreateMap<Services.Dtos.Log, Models.ViewModels.LogViewModel>();

            CreateMap<Services.Models.AppInfo, Models.ViewModels.AppVersionViewModel>();
            CreateMap<SpecialtyStudentsDispersion, Models.ViewModels.SpecialtyStudentsDispersionViewModel>();
            CreateMap<StudentScoreForDiscipline, Models.ViewModels.StudentScoreForDisciplineViewModel>();

            // info view models

            CreateMap<Services.Dtos.AcademicDisciplineInfo, Models.ViewModels.AcademicDisciplineInfoViewModel>();
            CreateMap<Services.Dtos.AcademicGroupInfo, Models.ViewModels.AcademicGroupInfoViewModel>();
            CreateMap<Services.Dtos.CurriculumInfo, Models.ViewModels.CurriculumInfoViewModel>();
            CreateMap<Services.Dtos.DepartmentInfo, Models.ViewModels.DepartmentInfoViewModel>();
            CreateMap<Services.Dtos.EducationProgramInfo, Models.ViewModels.EducationProgramInfoViewModel>();
            CreateMap<Services.Dtos.ExamsGradesSpreadsheetInfo, Models.ViewModels.ExamsGradesSpreadsheetInfoViewModel>();
            CreateMap<Services.Dtos.RatingForDisciplineInfo, Models.ViewModels.RatingForDisciplineInfoViewModel>();
            CreateMap<Services.Dtos.StudentInfo, Models.ViewModels.StudentInfoViewModel>();
            CreateMap<Services.Dtos.TeacherInfo, Models.ViewModels.TeacherInfoViewModel>();

            // bindings models

            // users
            CreateMap<Models.BindingModels.UserBindingModel, Services.Dtos.User>();
            CreateMap<Models.BindingModels.SignUpBindingModel, Services.Dtos.User>();
            // students
            CreateMap<Models.BindingModels.CreateStudentBindingModel, Services.Dtos.Student>();
            CreateMap<Models.BindingModels.UpdateStudentBindingModel, Services.Dtos.Student>();
            CreateMap<Models.BindingModels.PassportBindingModel, Services.Dtos.Passport>();
            CreateMap<Models.BindingModels.IdentificationCodeBindingModel, Services.Dtos.IdentificationCode>();
            CreateMap<Models.BindingModels.EducationDocumentBindingModel, Services.Dtos.EducationDocument>();
            // academic disciplines
            CreateMap<Models.BindingModels.CreateAcademicDisciplineBindingModel, Services.Dtos.AcademicDiscipline>();
            CreateMap<Models.BindingModels.UpdateAcademicDisciplineBindingModel, Services.Dtos.AcademicDiscipline>();
            // academic groups
            CreateMap<Models.BindingModels.CreateAcademicGroupBindingModel, Services.Dtos.AcademicGroup>();
            CreateMap<Models.BindingModels.UpdateAcademicGroupBindingModel, Services.Dtos.AcademicGroup>();
            // curricula
            CreateMap<Models.BindingModels.CreateCurriculumBindingModel, Services.Dtos.Curriculum>();
            CreateMap<Models.BindingModels.UpdateCurriculumBindingModel, Services.Dtos.Curriculum>();
            // departments
            CreateMap<Models.BindingModels.CreateDepartmentBindingModel, Services.Dtos.Department>();
            CreateMap<Models.BindingModels.UpdateDepartmentBindingModel, Services.Dtos.Department>();
            // education levels
            CreateMap<Models.BindingModels.CreateEducationLevelBindingModel, Services.Dtos.EducationLevel>();
            CreateMap<Models.BindingModels.UpdateEducationLevelBindingModel, Services.Dtos.EducationLevel>();
            // education programs
            CreateMap<Models.BindingModels.CreateEducationProgramBindingModel, Services.Dtos.EducationProgram>();
            CreateMap<Models.BindingModels.UpdateEducationProgramBindingModel, Services.Dtos.EducationProgram>();
            // exams grades spreadsheets
            CreateMap<Models.BindingModels.CreateExamsGradesSpreadsheetBindingModel, Services.Dtos.ExamsGradesSpreadsheet>();
            CreateMap<Models.BindingModels.UpdateExamsGradesSpreadsheetBindingModel, Services.Dtos.ExamsGradesSpreadsheet>();
            // forms of education
            CreateMap<Models.BindingModels.CreateFormOfEducationBindingModel, Services.Dtos.FormOfEducation>();
            CreateMap<Models.BindingModels.UpdateFormOfEducationBindingModel, Services.Dtos.FormOfEducation>();
            // permission
            CreateMap<Permission, Services.Dtos.Permission>();
            // privileges
            CreateMap<Models.BindingModels.CreatePrivilegeBindingModel, Services.Dtos.Privilege>();
            CreateMap<Models.BindingModels.UpdatePrivilegeBindingModel, Services.Dtos.Privilege>();
            // rating for disciplines
            CreateMap<Models.BindingModels.CreateRatingForDisciplineBindingModel, Services.Dtos.RatingForDiscipline>();
            CreateMap<Models.BindingModels.UpdateRatingForDisciplineBindingModel, Services.Dtos.RatingForDiscipline>();
            // roles
            CreateMap<Models.BindingModels.CreateRoleBindingModel, Services.Dtos.Role>();
            CreateMap<Models.BindingModels.UpdateRoleBindingModel, Services.Dtos.Role>();
            // specialties
            CreateMap<Models.BindingModels.CreateSpecialtyBindingModel, Services.Dtos.Specialty>();
            CreateMap<Models.BindingModels.UpdateSpecialtyBindingModel, Services.Dtos.Specialty>();
            // structural units
            CreateMap<Models.BindingModels.CreateStructuralUnitBindingModel, Services.Dtos.StructuralUnit>();
            CreateMap<Models.BindingModels.UpdateStructuralUnitBindingModel, Services.Dtos.StructuralUnit>();
            // teachers
            CreateMap<Models.BindingModels.CreateTeacherBindingModel, Services.Dtos.Teacher>();
            CreateMap<Models.BindingModels.UpdateTeacherBindingModel, Services.Dtos.Teacher>();
        }
    }
}
