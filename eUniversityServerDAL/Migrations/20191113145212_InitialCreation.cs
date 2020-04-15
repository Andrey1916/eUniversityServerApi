using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eUniversityServer.DAL.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EducationDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Series = table.Column<string>(nullable: false),
                    Number = table.Column<long>(nullable: false),
                    IssuingAuthority = table.Column<string>(maxLength: 512, nullable: false),
                    DateOfIssue = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: false),
                    Number = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormOfEducations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormOfEducations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentificationCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdentificationCodeNumber = table.Column<long>(nullable: false),
                    IdentificationCodeIssuingAuthority = table.Column<string>(maxLength: 512, nullable: false),
                    IdentificationCodeDateOfIssue = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentificationCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PassportSeries = table.Column<string>(maxLength: 2, nullable: false),
                    PassportNumber = table.Column<long>(nullable: false),
                    PassportIssuingAuthority = table.Column<string>(maxLength: 512, nullable: false),
                    PassportDateOfIssue = table.Column<DateTime>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    RegistrationAddress = table.Column<string>(nullable: true),
                    MaritalStatus = table.Column<string>(nullable: true),
                    PlaceOfBirth = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TargetModifier = table.Column<int>(unicode: false, nullable: false),
                    AccessModifier = table.Column<int>(unicode: false, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Privileges",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privileges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 96, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: false),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    GroupsCode = table.Column<string>(maxLength: 16, nullable: false),
                    Discipline = table.Column<string>(maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StructuralUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: true),
                    FullName = table.Column<string>(maxLength: 512, nullable: false),
                    FullNameEng = table.Column<string>(maxLength: 512, nullable: false),
                    ShortName = table.Column<string>(maxLength: 256, nullable: true),
                    Chief = table.Column<string>(maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StructuralUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Patronymic = table.Column<string>(nullable: true),
                    FirstNameEng = table.Column<string>(nullable: true),
                    LastNameEng = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(nullable: false),
                    PermissionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EducationPrograms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SpecialtyId = table.Column<Guid>(nullable: false),
                    EducationLevelId = table.Column<Guid>(nullable: false),
                    DurationbOfEducation = table.Column<short>(nullable: true),
                    Language = table.Column<string>(maxLength: 32, nullable: true),
                    ApprovalYear = table.Column<DateTime>(nullable: true),
                    Guarantor = table.Column<string>(maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationPrograms_EducationLevels_EducationLevelId",
                        column: x => x.EducationLevelId,
                        principalTable: "EducationLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EducationPrograms_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LevelsOfHigherEducationSpecialties",
                columns: table => new
                {
                    SpecialtyId = table.Column<Guid>(nullable: false),
                    EducationLevelId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelsOfHigherEducationSpecialties", x => new { x.SpecialtyId, x.EducationLevelId });
                    table.ForeignKey(
                        name: "FK_LevelsOfHigherEducationSpecialties_EducationLevels_EducationLevelId",
                        column: x => x.EducationLevelId,
                        principalTable: "EducationLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LevelsOfHigherEducationSpecialties_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StructuralUnitId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(maxLength: 16, nullable: true),
                    ShortName = table.Column<string>(maxLength: 256, nullable: true),
                    FullName = table.Column<string>(maxLength: 512, nullable: true),
                    FullNameEng = table.Column<string>(maxLength: 512, nullable: true),
                    Chief = table.Column<string>(maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_StructuralUnits_StructuralUnitId",
                        column: x => x.StructuralUnitId,
                        principalTable: "StructuralUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: false),
                    PasswordSalt = table.Column<byte[]>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UserInfoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcademicGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SpecialtyId = table.Column<Guid>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: false),
                    StructuralUnitId = table.Column<Guid>(nullable: false),
                    EducationLevelId = table.Column<Guid>(nullable: false),
                    FormOfEducationId = table.Column<Guid>(nullable: false),
                    UIN = table.Column<string>(maxLength: 256, nullable: false),
                    Code = table.Column<string>(maxLength: 12, nullable: false),
                    Grade = table.Column<short>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Curator = table.Column<string>(nullable: true),
                    Captain = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicGroups_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicGroups_EducationLevels_EducationLevelId",
                        column: x => x.EducationLevelId,
                        principalTable: "EducationLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicGroups_FormOfEducations_FormOfEducationId",
                        column: x => x.FormOfEducationId,
                        principalTable: "FormOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicGroups_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicGroups_StructuralUnits_StructuralUnitId",
                        column: x => x.StructuralUnitId,
                        principalTable: "StructuralUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Curricula",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SpecialtyId = table.Column<Guid>(nullable: false),
                    StructuralUnitId = table.Column<Guid>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: false),
                    FormOfEducationId = table.Column<Guid>(nullable: false),
                    EducationProgramId = table.Column<Guid>(nullable: true),
                    EducationLevelId = table.Column<Guid>(nullable: false),
                    YearOfAdmission = table.Column<int>(nullable: true),
                    DateOfApproval = table.Column<DateTime>(nullable: true),
                    ScheduleOfEducationProcess = table.Column<string>(nullable: true),
                    ListOfApprovals = table.Column<string>(nullable: true),
                    OrderOfApprovals = table.Column<string>(nullable: true),
                    ProtocolOfAcademicCouncilOfUnit = table.Column<string>(nullable: true),
                    ProtocolOfAcademicCouncilOfUniversity = table.Column<string>(nullable: true),
                    SpecialtyGuarantor = table.Column<string>(maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curricula", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Curricula_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Curricula_EducationLevels_EducationLevelId",
                        column: x => x.EducationLevelId,
                        principalTable: "EducationLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Curricula_EducationPrograms_EducationProgramId",
                        column: x => x.EducationProgramId,
                        principalTable: "EducationPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Curricula_FormOfEducations_FormOfEducationId",
                        column: x => x.FormOfEducationId,
                        principalTable: "FormOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Curricula_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Curricula_StructuralUnits_StructuralUnitId",
                        column: x => x.StructuralUnitId,
                        principalTable: "StructuralUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: false),
                    UserInfoId = table.Column<Guid>(nullable: false),
                    Position = table.Column<string>(maxLength: 256, nullable: true),
                    ScientificDegree = table.Column<string>(maxLength: 256, nullable: true),
                    AcademicRank = table.Column<string>(maxLength: 256, nullable: true),
                    TypeOfEmployment = table.Column<int>(unicode: false, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teachers_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    Message = table.Column<string>(nullable: false),
                    LogLevel = table.Column<int>(unicode: false, nullable: false),
                    StackTrace = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserInfoId = table.Column<Guid>(nullable: false),
                    PrivilegeId = table.Column<Guid>(nullable: true),
                    AcademicGroupId = table.Column<Guid>(nullable: false),
                    EducationProgramId = table.Column<Guid>(nullable: true),
                    FormOfEducationId = table.Column<Guid>(nullable: false),
                    EducationLevelId = table.Column<Guid>(nullable: false),
                    IdentificationCodeId = table.Column<Guid>(nullable: false),
                    PassportId = table.Column<Guid>(nullable: false),
                    EducationDocumentId = table.Column<Guid>(nullable: false),
                    Sex = table.Column<int>(unicode: false, nullable: false),
                    StudentTicketNumber = table.Column<long>(nullable: false),
                    Financing = table.Column<int>(unicode: false, nullable: false),
                    AddressOfResidence = table.Column<string>(maxLength: 512, nullable: true),
                    NumberOfRecordBook = table.Column<int>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ForeignLanguage = table.Column<string>(maxLength: 128, nullable: true),
                    MilitaryRegistration = table.Column<bool>(nullable: true),
                    Chummery = table.Column<string>(maxLength: 512, nullable: true),
                    AcceleratedFormOfEducation = table.Column<bool>(maxLength: 512, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_AcademicGroups_AcademicGroupId",
                        column: x => x.AcademicGroupId,
                        principalTable: "AcademicGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_EducationDocuments_EducationDocumentId",
                        column: x => x.EducationDocumentId,
                        principalTable: "EducationDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_EducationLevels_EducationLevelId",
                        column: x => x.EducationLevelId,
                        principalTable: "EducationLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_EducationPrograms_EducationProgramId",
                        column: x => x.EducationProgramId,
                        principalTable: "EducationPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_FormOfEducations_FormOfEducationId",
                        column: x => x.FormOfEducationId,
                        principalTable: "FormOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_IdentificationCodes_IdentificationCodeId",
                        column: x => x.IdentificationCodeId,
                        principalTable: "IdentificationCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Passports_PassportId",
                        column: x => x.PassportId,
                        principalTable: "Passports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Privileges_PrivilegeId",
                        column: x => x.PrivilegeId,
                        principalTable: "Privileges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcademicDisciplines",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SpecialtyId = table.Column<Guid>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: false),
                    CurriculumId = table.Column<Guid>(nullable: false),
                    LecturerId = table.Column<Guid>(nullable: false),
                    AssistantId = table.Column<Guid>(nullable: false),
                    FullName = table.Column<string>(maxLength: 512, nullable: false),
                    ShortName = table.Column<string>(maxLength: 256, nullable: false),
                    Semester = table.Column<int>(unicode: false, nullable: false),
                    NumberOfCredits = table.Column<int>(nullable: false),
                    Attestation = table.Column<int>(unicode: false, nullable: false),
                    TypeOfIndividualWork = table.Column<int>(unicode: false, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicDisciplines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicDisciplines_Teachers_AssistantId",
                        column: x => x.AssistantId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicDisciplines_Curricula_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curricula",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicDisciplines_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicDisciplines_Teachers_LecturerId",
                        column: x => x.LecturerId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicDisciplines_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamsGradesSpreadsheets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SpecialtyId = table.Column<Guid>(nullable: false),
                    FormOfEducationId = table.Column<Guid>(nullable: false),
                    AcademicGroupId = table.Column<Guid>(nullable: false),
                    AcademicDisciplineId = table.Column<Guid>(nullable: false),
                    EducationProgramId = table.Column<Guid>(nullable: false),
                    StructuralUnitId = table.Column<Guid>(nullable: false),
                    SpreadsheetNumber = table.Column<string>(maxLength: 512, nullable: false),
                    SemesterNumber = table.Column<short>(nullable: false),
                    ExamDate = table.Column<DateTime>(nullable: false),
                    ExamsSpreadsheetType = table.Column<int>(unicode: false, nullable: false),
                    ExamsSpreadsheetAttestationType = table.Column<int>(unicode: false, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamsGradesSpreadsheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamsGradesSpreadsheets_AcademicDisciplines_AcademicDisciplineId",
                        column: x => x.AcademicDisciplineId,
                        principalTable: "AcademicDisciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamsGradesSpreadsheets_AcademicGroups_AcademicGroupId",
                        column: x => x.AcademicGroupId,
                        principalTable: "AcademicGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamsGradesSpreadsheets_EducationPrograms_EducationProgramId",
                        column: x => x.EducationProgramId,
                        principalTable: "EducationPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamsGradesSpreadsheets_FormOfEducations_FormOfEducationId",
                        column: x => x.FormOfEducationId,
                        principalTable: "FormOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamsGradesSpreadsheets_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamsGradesSpreadsheets_StructuralUnits_StructuralUnitId",
                        column: x => x.StructuralUnitId,
                        principalTable: "StructuralUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RatingForDisciplines",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExamsGradesSpreadsheetId = table.Column<Guid>(nullable: false),
                    AcademicDisciplineId = table.Column<Guid>(nullable: false),
                    AcademicGroupId = table.Column<Guid>(nullable: false),
                    TeacherId = table.Column<Guid>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    Score = table.Column<short>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingForDisciplines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RatingForDisciplines_AcademicDisciplines_AcademicDisciplineId",
                        column: x => x.AcademicDisciplineId,
                        principalTable: "AcademicDisciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RatingForDisciplines_AcademicGroups_AcademicGroupId",
                        column: x => x.AcademicGroupId,
                        principalTable: "AcademicGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RatingForDisciplines_ExamsGradesSpreadsheets_ExamsGradesSpreadsheetId",
                        column: x => x.ExamsGradesSpreadsheetId,
                        principalTable: "ExamsGradesSpreadsheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RatingForDisciplines_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RatingForDisciplines_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "AccessModifier", "CreatedAt", "TargetModifier", "UpdatedAt" },
                values: new object[] { new Guid("031e06df-651d-4791-85d1-3d9f0f3fa320"), 1, new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc), 0, new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("031e06df-651d-4791-85d1-3d9f0f3fa320"), new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc), "SuperAdmin", new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc) },
                    { new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc), "User", new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "UserInfos",
                columns: new[] { "Id", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FirstNameEng", "LastName", "LastNameEng", "Patronymic", "PhoneNumber" },
                values: new object[] { new Guid("031e06df-651d-4791-87d1-3d9f0f8fa325"), null, "admin@euniversity.com", true, "John", "John", "Doe", "Doe", "Smith", null });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[] { new Guid("031e06df-651d-4791-85d1-3d9f0f3fa320"), new Guid("031e06df-651d-4791-85d1-3d9f0f3fa320") });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "PasswordSalt", "UpdatedAt", "UserInfoId" },
                values: new object[] { new Guid("b0b0fec5-8958-43ca-99b8-1978f198cf06"), new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc), new byte[] { 89, 33, 171, 24, 175, 100, 4, 18, 135, 159, 201, 244, 109, 25, 111, 95, 55, 6, 204, 243, 71, 220, 125, 169, 197, 218, 207, 13, 6, 226, 200, 38, 115, 11, 128, 249, 118, 17, 149, 21, 29, 213, 64, 232, 238, 39, 238, 161, 210, 85, 241, 126, 29, 231, 84, 113, 148, 64, 50, 176, 129, 5, 112, 33 }, new byte[] { 254, 195, 179, 232, 139, 63, 110, 232, 152, 23, 132, 215, 28, 175, 142, 119, 12, 137, 45, 36, 231, 209, 44, 24, 245, 183, 125, 33, 7, 0, 119, 208, 4, 68, 203, 137, 69, 147, 100, 10, 100, 191, 249, 249, 205, 62, 227, 218, 159, 127, 219, 11, 20, 200, 102, 50, 236, 50, 48, 52, 115, 34, 89, 230, 36, 102, 242, 205, 163, 140, 107, 81, 232, 114, 251, 152, 241, 13, 81, 110, 12, 197, 11, 163, 39, 104, 186, 184, 181, 61, 130, 95, 72, 198, 16, 139, 104, 133, 32, 15, 126, 204, 169, 44, 248, 11, 129, 162, 180, 202, 126, 174, 129, 155, 86, 45, 139, 115, 170, 125, 227, 138, 52, 163, 132, 144, 89, 155 }, new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc), new Guid("031e06df-651d-4791-87d1-3d9f0f8fa325") });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("031e06df-651d-4791-85d1-3d9f0f3fa320"), new Guid("b0b0fec5-8958-43ca-99b8-1978f198cf06") });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicDisciplines_AssistantId",
                table: "AcademicDisciplines",
                column: "AssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicDisciplines_CurriculumId",
                table: "AcademicDisciplines",
                column: "CurriculumId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicDisciplines_DepartmentId",
                table: "AcademicDisciplines",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicDisciplines_LecturerId",
                table: "AcademicDisciplines",
                column: "LecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicDisciplines_SpecialtyId",
                table: "AcademicDisciplines",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicGroups_DepartmentId",
                table: "AcademicGroups",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicGroups_EducationLevelId",
                table: "AcademicGroups",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicGroups_FormOfEducationId",
                table: "AcademicGroups",
                column: "FormOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicGroups_SpecialtyId",
                table: "AcademicGroups",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicGroups_StructuralUnitId",
                table: "AcademicGroups",
                column: "StructuralUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Curricula_DepartmentId",
                table: "Curricula",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Curricula_EducationLevelId",
                table: "Curricula",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Curricula_EducationProgramId",
                table: "Curricula",
                column: "EducationProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Curricula_FormOfEducationId",
                table: "Curricula",
                column: "FormOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_Curricula_SpecialtyId",
                table: "Curricula",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Curricula_StructuralUnitId",
                table: "Curricula",
                column: "StructuralUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_StructuralUnitId",
                table: "Departments",
                column: "StructuralUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationPrograms_EducationLevelId",
                table: "EducationPrograms",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationPrograms_SpecialtyId",
                table: "EducationPrograms",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamsGradesSpreadsheets_AcademicDisciplineId",
                table: "ExamsGradesSpreadsheets",
                column: "AcademicDisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamsGradesSpreadsheets_AcademicGroupId",
                table: "ExamsGradesSpreadsheets",
                column: "AcademicGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamsGradesSpreadsheets_EducationProgramId",
                table: "ExamsGradesSpreadsheets",
                column: "EducationProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamsGradesSpreadsheets_FormOfEducationId",
                table: "ExamsGradesSpreadsheets",
                column: "FormOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamsGradesSpreadsheets_SpecialtyId",
                table: "ExamsGradesSpreadsheets",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamsGradesSpreadsheets_SpreadsheetNumber",
                table: "ExamsGradesSpreadsheets",
                column: "SpreadsheetNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamsGradesSpreadsheets_StructuralUnitId",
                table: "ExamsGradesSpreadsheets",
                column: "StructuralUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_LevelsOfHigherEducationSpecialties_EducationLevelId",
                table: "LevelsOfHigherEducationSpecialties",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UserId",
                table: "Logs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingForDisciplines_AcademicDisciplineId",
                table: "RatingForDisciplines",
                column: "AcademicDisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingForDisciplines_AcademicGroupId",
                table: "RatingForDisciplines",
                column: "AcademicGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingForDisciplines_ExamsGradesSpreadsheetId",
                table: "RatingForDisciplines",
                column: "ExamsGradesSpreadsheetId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingForDisciplines_StudentId",
                table: "RatingForDisciplines",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingForDisciplines_TeacherId",
                table: "RatingForDisciplines",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_AcademicGroupId",
                table: "Students",
                column: "AcademicGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_EducationDocumentId",
                table: "Students",
                column: "EducationDocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_EducationLevelId",
                table: "Students",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_EducationProgramId",
                table: "Students",
                column: "EducationProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_FormOfEducationId",
                table: "Students",
                column: "FormOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_IdentificationCodeId",
                table: "Students",
                column: "IdentificationCodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_PassportId",
                table: "Students",
                column: "PassportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_PrivilegeId",
                table: "Students",
                column: "PrivilegeId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserInfoId",
                table: "Students",
                column: "UserInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DepartmentId",
                table: "Teachers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserInfoId",
                table: "Teachers",
                column: "UserInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserInfoId",
                table: "Users",
                column: "UserInfoId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LevelsOfHigherEducationSpecialties");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "RatingForDisciplines");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "ExamsGradesSpreadsheets");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AcademicDisciplines");

            migrationBuilder.DropTable(
                name: "AcademicGroups");

            migrationBuilder.DropTable(
                name: "EducationDocuments");

            migrationBuilder.DropTable(
                name: "IdentificationCodes");

            migrationBuilder.DropTable(
                name: "Passports");

            migrationBuilder.DropTable(
                name: "Privileges");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Curricula");

            migrationBuilder.DropTable(
                name: "UserInfos");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "EducationPrograms");

            migrationBuilder.DropTable(
                name: "FormOfEducations");

            migrationBuilder.DropTable(
                name: "StructuralUnits");

            migrationBuilder.DropTable(
                name: "EducationLevels");

            migrationBuilder.DropTable(
                name: "Specialties");
        }
    }
}
