using System;
using System.ComponentModel;

namespace eUniversityServer.DAL.Enums
{
    public enum IndividualWork
    {
        [Description("РГР")]
        RGR,
        [Description("КР")]
        KR,
        [Description("КП")]
        KP
    }

    public enum ExamsSpreadsheetType
    {
        [Description("Основна")]
        Main,
        [Description("Додаткова")]
        Additional
    }

    public enum Financing
    {
        [Description("Бюджет")]
        Budget = 0,
        [Description("Контракт")]
        Contract = 1
    }

    public enum Employment
    {
        [Description("Штатний")]
        FullTime = 0,
        [Description("Сумісник")]
        PartTime = 1
    }

    public enum SemesterType
    {
        [Description("Весінній")]
        Spring,
        [Description("Осінній")]
        Autumn
    }

    public enum AttestationType
    {
        [Description("Залік")]
        Test,
        [Description("Іспит")]
        Exam
    }

    public enum SexType
    {
        [Description("Чоловік")]
        Male,
        [Description("Жінка")]
        Female
    }

    public enum ExamsSpreadsheetAttestationType
    {
        [Description("Залік")]
        Test,
        [Description("Іспит")]
        Exam,
        [Description("КР")]
        KR
    }

    public enum Level
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    [Flags]
    public enum AccessModifier : int
    {
        CanRead   = 1,
        CanUpdate = 2,
        CanCreate = 4,
        CanDelete = 8,
    }

    public enum TargetModifier
    {
        AcademicDisciplines,
        AcademicGroups,
        Curricula,
        Departments,
        //EducationDocuments,
        EducationLevels,
        EducationPrograms,
        ExamsGradesSpreadsheets,
        FormsOfEducation,
        //IdentificationCodes,
        //Logs,
        //Passports,
        Permissions,
        Privileges,
        RatingsForDisciplines,
        Roles,
        Specialties,
        StructuralUnits,
        Students,
        StudentDocuments,
        Teachers,
        Users
    }
}
