using AutoMapper;
using eUniversityServer.DAL;
using eUniversityServer.Services;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Utils;
using eUniversityServer.Services.Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sieve.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using eUniversityServer.Services.Utils;
using Sieve.Models;
using eUniversityServer.Utils.Auth;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Internal;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace eUniversityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            
            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddAutoMapper();

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "_af";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.HeaderName = "X-XSRF-Token";
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<Utils.AppSettings>(appSettingsSection);

            services.Configure<SieveOptions>(Configuration.GetSection("Sieve"));

            #region Database
#if DEBUG
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);
#else
            // configure database connection
            string awsConnection = GetRDSConnectionString();
            services.AddDbContext<DataContext>(options => options.UseSqlServer(awsConnection));
#endif
            #endregion

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();

            #region JWT
            var key = Encoding.UTF8.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var claims = context.Principal.Identity as ClaimsIdentity;

                        var userIdClaim = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                        if (userIdClaim == null)
                        {
                            context.Fail("Invalid token (user id not found)");
                            return;
                        }

                        var sessionIdClaim = claims.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);

                        if (sessionIdClaim == null)
                        {
                            context.Fail("Invalid token (session id not found)");
                            return;
                        }

                        var userId    = Guid.Parse(userIdClaim.Value);
                        var sessionId = Guid.Parse(sessionIdClaim.Value);

                        var user = await userService.GetByIdAsync(userId);

                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Invalid token (user not found)");
                            return;
                        }
                        
                        if (!userService.CheckSession(userId, sessionId))
                        {
                            context.Fail("Unauthorized");
                            return;
                        }

                        context.Success();
                    }
                };
                
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer              = appSettings.Issuer,
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidAudience            = appSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey         = new SymmetricSecurityKey(key),
                    RequireExpirationTime    = true,
                    ValidateLifetime         = true
                };
            });
            #endregion

            #region DI
            // set auth policies
            services.AddAuthorization(options => AppPolicies.SetPolicies(options));

            // utils
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddScoped<IAuthorizationService, DefaultAuthorizationService>();

            // sieve
            services.AddScoped<SieveProcessor, ApplicationSieveProcessor>();
            services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>();

            services.AddScoped<DbContext, DataContext>();
            services.AddScoped<IEmailProvider, SendGridEmailProvider>();
            services.AddScoped<Logger, DbLogger>();
            services.AddScoped<IAppSettings, AppSettings>(serviceProvider => appSettings);

            // configure DI for application services
            services.AddScoped<IAcademicDisciplineService, AcademicDisciplineService>();
            services.AddScoped<IAcademicGroupService, AcademicGroupService>();
            services.AddScoped<ICurriculumService, CurriculumService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEducationLevelService, EducationLevelService>();
            services.AddScoped<IEducationProgramService, EducationProgramService>();
            services.AddScoped<IExamsGradesSpreadsheetService, ExamsGradesSpreadsheetService>();
            services.AddScoped<IFormOfEducationService, FormOfEducationService>();
            services.AddScoped<IPrivilegeService, PrivilegeService>();
            services.AddScoped<IRatingForDisciplineService, RatingForDisciplineService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<IStructuralUnitService, StructuralUnitService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IAppVersionsService, AppVersionsService>();
            services.AddScoped<IStatisticsService, StatisticsService>();

            #endregion

            #region Swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "eUniversity API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Please insert JWT with Bearer field",
                        Name = "Authorization",
                        Type = "apiKey"
                    });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });

                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

                c.IncludeXmlComments(xmlPath);
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else app.UseHsts();

            app.UseCors();

            app.UseSwagger( option => { option.RouteTemplate = "swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "eUniversity API");
                c.RoutePrefix = "swagger";
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }

        public static string GetRDSConnectionString()
        {
            var appConfig = ConfigurationManager.AppSettings;

            string dbname = appConfig["RDS_DB_NAME"];
            if (string.IsNullOrEmpty(dbname))
                return null;

            string username = appConfig["RDS_USERNAME"];
            string password = appConfig["RDS_PASSWORD"];
            string hostname = appConfig["RDS_HOSTNAME"];
            string port = appConfig["RDS_PORT"];

            return $"Data Source={hostname};Initial Catalog={dbname};User ID={username};Password={password};";
        }
    }
}
