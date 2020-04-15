using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace eUniversityServer.DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.AcademicDisciplineConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.AcademicGroupConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.CurriculumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.EducationDocumentConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.EducationLevelConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.EducationProgramConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ExamsGradesSpreadsheetConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.FormOfEducationConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.IdentificationCodeConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.LogConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PassportConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PrivilegeConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.RatingForDisciplineConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.RoleConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.RolePermissionsConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.LevelsOfHigherEducationSpecialtiesConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SpecialtyConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.StructuralUnitConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.StudentConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.UserRolesConfiguration());

            modelBuilder.Entity<Entities.UserInfo>().HasData(new Entities.UserInfo
            {
                Id             = Guid.Parse("031e06df-651d-4791-87d1-3d9f0f8fa325"),
                Email          = "admin@euniversity.com",
                EmailConfirmed = true,
                FirstName      = "John",
                LastName       = "Doe",
                FirstNameEng   = "John",
                LastNameEng    = "Doe",
                Patronymic     = "Smith"
            });

            string password = "q_1w2e3r4t5y6";

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                modelBuilder.Entity<Entities.User>().HasData(new Entities.User
                {
                    Id           = Guid.Parse("b0b0fec5-8958-43ca-99b8-1978f198cf06"),
                    CreatedAt    = new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc),
                    UpdatedAt    = new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                    PasswordSalt = hmac.Key,
                    UserInfoId   = Guid.Parse("031e06df-651d-4791-87d1-3d9f0f8fa325")
                });
            }

            modelBuilder.Entity<Entities.Role>().HasData(new Entities.Role
            {
                Id        = Guid.Parse("031e06df-651d-4791-85d1-3d9f0f3fa320"),
                Name      = "SuperAdmin",
                CreatedAt = new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc)
            });

            modelBuilder.Entity<Entities.Role>().HasData(new Entities.Role
            {
                Id        = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
                Name      = "User",
                CreatedAt = new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc)
            });

            modelBuilder.Entity<Entities.UserRoles>().HasData(
                new Entities.UserRoles
                {
                    RoleId = Guid.Parse("031e06df-651d-4791-85d1-3d9f0f3fa320"),
                    UserId = Guid.Parse("b0b0fec5-8958-43ca-99b8-1978f198cf06")
                }
            );

            modelBuilder.Entity<Entities.Permission>().HasData(
                new Entities.Permission
                {
                    Id             = Guid.Parse("031e06df-651d-4791-85d1-3d9f0f3fa320"),
                    AccessModifier = Enums.AccessModifier.CanRead,
                    TargetModifier = Enums.TargetModifier.AcademicDisciplines,
                    CreatedAt      = new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc),
                    UpdatedAt      = new DateTime(2019, 5, 25, 13, 6, 26, 0, DateTimeKind.Utc)
                }
                );

            modelBuilder.Entity<Entities.RolePermissions>().HasData(
                new Entities.RolePermissions
                {
                    PermissionId = Guid.Parse("031e06df-651d-4791-85d1-3d9f0f3fa320"),
                    RoleId       = Guid.Parse("031e06df-651d-4791-85d1-3d9f0f3fa320")
                }
                );
        }

        public DbSet<Entities.AcademicDiscipline> AcademicDisciplines { get; set; }
        public DbSet<Entities.AcademicGroup> AcademicGroups { get; set; }
        public DbSet<Entities.Curriculum> Curricula { get; set; }
        public DbSet<Entities.Department> Departments { get; set; }
        public DbSet<Entities.EducationDocument> EducationDocuments { get; set; }
        public DbSet<Entities.EducationLevel> EducationLevels { get; set; }
        public DbSet<Entities.EducationProgram> EducationPrograms { get; set; }
        public DbSet<Entities.ExamsGradesSpreadsheet> ExamsGradesSpreadsheets { get; set; }
        public DbSet<Entities.FormOfEducation> FormOfEducations { get; set; }
        public DbSet<Entities.IdentificationCode> IdentificationCodes { get; set; }
        public DbSet<Entities.Log> Logs { get; set; }
        public DbSet<Entities.Passport> Passports { get; set; }
        public DbSet<Entities.Permission> Permissions { get; set; }
        public DbSet<Entities.Privilege> Privileges { get; set; }
        public DbSet<Entities.RatingForDiscipline> RatingForDisciplines { get; set; }
        public DbSet<Entities.Role> Roles { get; set; }
        public DbSet<Entities.RolePermissions> RolePermissions { get; set; }
        public DbSet<Entities.Specialty> Specialties { get; set; }
        public DbSet<Entities.LevelsOfHigherEducationSpecialties> LevelsOfHigherEducationSpecialties { get; set; }
        public DbSet<Entities.StructuralUnit> StructuralUnits { get; set; }
        public DbSet<Entities.Student> Students { get; set; }
        public DbSet<Entities.Teacher> Teachers { get; set; }
        public DbSet<Entities.UserInfo> UserInfos { get; set; }
        public DbSet<Entities.User> Users { get; set; }
        public DbSet<Entities.UserRoles> UserRoles { get; set; }
    }
}
