using eUniversityServer.DAL;
using eUniversityServer.DAL.Enums;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Models;
using eUniversityServer.Services.Utils;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Entities = eUniversityServer.DAL.Entities;
using System.Threading;

namespace eUniversityServer.Services
{
    public class UserService : IUserService
    {
        private enum TokenType
        {
            MailConfirmation,
            ResetPassword
        }

        private struct SessionInfo
        {
            public Guid userId;
            public Guid sessionId;
            public DateTime expires;
            public Guid refreshTokenId;
        }

        private struct TokenInfo
        {
            public Guid userId;
            public Guid actionId;
            public DateTime expires;
            public TokenType type;
        }

        private static object syncSessionObj = new object();
        private static object syncTokenObj = new object();

        private static readonly List<SessionInfo> sessions = new List<SessionInfo>();
        private static readonly List<TokenInfo> tokens = new List<TokenInfo>();

        private readonly DbContext _context;
        private readonly IAppSettings _appSettings;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly UserInfoService _userInfoService;
        private readonly IEmailProvider _emailProvider;

        public UserService(IAppSettings appSettings, DbContext context, SieveProcessor sieveProcessor, IEmailProvider emailProvider)
        {
            _context        = context ?? throw new NullReferenceException(nameof(context));
            _appSettings    = appSettings ?? throw new NullReferenceException(nameof(appSettings));
            _sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
            _emailProvider  = emailProvider ?? throw new NullReferenceException(nameof(emailProvider));

            this._userInfoService = new UserInfoService(context);
        }
        

        public async Task<Dtos.UserWithToken> SignInAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new AuthenticationException("Email is required");

            if (string.IsNullOrWhiteSpace(password))
                throw new AuthenticationException("Password is required");

            var user = await _context.Set<Entities.User>()
                                     .Include(x => x.UserInfo)
                                     .Include(x => x.UserRoles)
                                     .ThenInclude(ur => ur.Role)
                                     .ThenInclude(r => r.RolePermissions)
                                     .ThenInclude(rp => rp.Permission)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(x => x.UserInfo.Email == email);
            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            
            if (!user.UserInfo.EmailConfirmed)
                throw new AuthenticationException("Email has not been confirmed");

            lock (syncSessionObj)
            {
                bool userHasSession = sessions.Any(s => s.userId == user.Id);

                if (userHasSession)
                {
                    sessions.Remove(
                        sessions.FirstOrDefault(s => s.userId == user.Id)
                        );
                }
            }

            // authentication successful so generate jwt token
            this.CreateTokens(user, out string accessToken, out string refreshToken);

            var userDto = new Dtos.UserWithToken
            {
                Id           = user.Id,
                TokenInfo    = new Dtos.TokenInfo
                {
                    AccessToken  = "Bearer " + accessToken,
                    RefreshToken = refreshToken,
                    Expire       = _appSettings.AccessTokenLifeTime
                },

                Roles        = user.UserRoles.GroupBy(x => x.Role)
                                             .Select(r => new Dtos.Role
                                             {
                                                 Id   = r.Key.Id,
                                                 Name = r.Key.Name,
                                                 Permissions = r.Key.RolePermissions.GroupBy(rp => rp.Permission)
                                                                                    .Select(perm => new Dtos.Permission 
                                                                                    {
                                                                                        Id = perm.Key.Id,
                                                                                        AccessModifier = perm.Key.AccessModifier,
                                                                                        TargetModifier = perm.Key.TargetModifier
                                                                                    })
                                             }),
                CreatedAt    = user.CreatedAt,
                DateOfBirth  = user.UserInfo.DateOfBirth,
                Email        = user.UserInfo.Email,
                FirstName    = user.UserInfo.FirstName,
                FirstNameEng = user.UserInfo.FirstNameEng,
                LastName     = user.UserInfo.LastName,
                LastNameEng  = user.UserInfo.LastNameEng,
                Patronymic   = user.UserInfo.Patronymic,
                PhoneNumber  = user.UserInfo.PhoneNumber,
                UpdatedAt    = user.UpdatedAt
                
            };

#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
            Task.Run(CheckAllSessions);
#pragma warning restore CS4014

            return userDto;
        }

        public async Task<Guid> SignUpAsync(Dtos.User userDto, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new RegistrationException("Password is required");

            var validator = new Dtos.UserValidator();
            ValidationResult result = validator.Validate(userDto);

            if (!result.IsValid)
            {
                string errMess = string.Empty;

                foreach (var failure in result.Errors)
                {
                    errMess += $"Property { failure.PropertyName } failed validation. Error was: { failure.ErrorMessage }\n";
                }

                throw new InvalidModelException(errMess);
            }

            bool isUserExists = _context.Set<Entities.User>().Include(x => x.UserInfo)
                                                             .Any(x => x.UserInfo != null && x.UserInfo.Email == userDto.Email);
            if (isUserExists)
            {
                throw new ServiceException(System.Net.HttpStatusCode.Conflict, $"Email \"{userDto.Email}\" is already taken");
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var userInfo = new Entities.UserInfo
            {
                Id           = Guid.NewGuid(),
                DateOfBirth  = userDto.DateOfBirth,
                Email        = userDto.Email,
                FirstName    = userDto.FirstName,
                FirstNameEng = userDto.FirstNameEng,
                LastName     = userDto.LastName,
                LastNameEng  = userDto.LastNameEng,
                Patronymic   = userDto.Patronymic,
                PhoneNumber  = userDto.PhoneNumber
            };
            await _context.AddAsync(userInfo);

            var user = new Entities.User
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt    = DateTime.UtcNow,
                UpdatedAt    = DateTime.UtcNow,
                Id           = Guid.NewGuid(),
                UserInfo     = userInfo
            };

            var role = await _context.Set<Entities.Role>()
                                     .FirstOrDefaultAsync(x => x.Name == AppRoles.User);
            if (role == null)
            {
                role = new Entities.Role
                {
                    Id        = Guid.NewGuid(),
                    Name      = AppRoles.User,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Set<Entities.Role>().AddAsync(role);
            }

            var userRole = new Entities.UserRoles
            {
                RoleId = role.Id,
                UserId = user.Id
            };

            await _context.Set<Entities.User>().AddAsync(user);
            await _context.Set<Entities.UserRoles>().AddAsync(userRole);
            await _context.SaveChangesAsync();

            return user.Id;
        }


        public async Task SendPasswordRecoveryEmailAsync(string url, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidModelException("Email is empty");
            }

            var user = await _context.Set<Entities.User>()
                                     .Include(x => x.UserInfo)
                                     .FirstOrDefaultAsync(x => x.UserInfo.Email == email);
            
            if (user == null)
            {
                throw new NotFoundException($"User with email { email } not found");
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);

            var claimes = new List<Claim>();

            Guid actionId = Guid.NewGuid();

            claimes.Add(new Claim("uid", user.Id.ToString()));
            claimes.Add(new Claim("aid", actionId.ToString()));

            var actionTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject            = new ClaimsIdentity(claimes),
                Expires            = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience           = _appSettings.Audience,
                Issuer             = _appSettings.Issuer
            };

            var acToken     = tokenHandler.CreateToken(actionTokenDescriptor);
            var actionToken = tokenHandler.WriteToken(acToken);

            lock (syncTokenObj)
            {
                tokens.Add(new TokenInfo
                {
                    actionId = actionId,
                    userId   = user.Id,
                    expires  = DateTime.UtcNow.AddMinutes(30),
                    type     = TokenType.ResetPassword
                });
            }

            string callbackUrl = url + actionToken;

            try
            {
                bool wasSent = _emailProvider.SendForgotPasswordMail(email, user.UserInfo.LastName + ' ' + user.UserInfo.FirstName, callbackUrl);

                if (!wasSent)
                {
                    throw new SendMailException("The mail was not sent");
                }
            }
            catch (Exception ex)
            {
                throw new SendMailException(ex.Message, ex);
            }
        }

        public Task RecoveryPasswordAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task ResetPasswordAsync(string token)
        {
            throw new NotImplementedException();
        }


        public async Task SendConfirmationEmailAsync(string domain, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidModelException("Email is empty");
            }

            var user = await _context.Set<Entities.User>()
                                     .Include(x => x.UserInfo)
                                     .FirstOrDefaultAsync(x => x.UserInfo.Email == email);
            if (user == null)
            {
                throw new NotFoundException($"User with email { email } not found");
            }

            if (user.UserInfo.EmailConfirmed == true)
            {
                throw new CanceledException(System.Net.HttpStatusCode.Forbidden, "Email is already confirmed");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);

            var claimes = new List<Claim>();

            Guid actionId = Guid.NewGuid();

            claimes.Add(new Claim("uid", user.Id.ToString()));
            claimes.Add(new Claim("aid", actionId.ToString()));

            var actionTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject            = new ClaimsIdentity(claimes),
                Expires            = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience           = _appSettings.Audience,
                Issuer             = _appSettings.Issuer
            };

            var acToken = tokenHandler.CreateToken(actionTokenDescriptor);
            var actionToken = tokenHandler.WriteToken(acToken);

            lock (syncTokenObj)
            {
                tokens.Add(new TokenInfo
                {
                    actionId = actionId,
                    userId   = user.Id,
                    expires  = DateTime.UtcNow.AddMinutes(30),
                    type     = TokenType.MailConfirmation
                });
            }

            string callbackUrl = $"{ domain }/{ actionToken }";

            try
            {
                bool wasSent = _emailProvider.SendEmailConfirmationMail(user.UserInfo.Email, user.UserInfo.FirstName + ' ' + user.UserInfo.LastName, callbackUrl);

                if (!wasSent)
                {
                    throw new SendMailException("The mail was not sent");
                }
            }
            catch (Exception ex)
            {
                throw new SendMailException(ex.Message, ex);
            }
        }

        public async Task ConfirmEmailAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidModelException("Token is empty");
            }

            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer              = _appSettings.Issuer,
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidAudience            = _appSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret)),
                RequireExpirationTime    = true,
                ValidateLifetime         = true
            };

            JwtSecurityToken jwt;

            try
            {
                var claimsPrincipal = new JwtSecurityTokenHandler()
                    .ValidateToken(token, validationParameters, out var rawValidatedToken);

                jwt = (JwtSecurityToken)rawValidatedToken;
            }
            catch (SecurityTokenValidationException stvex)
            {
                throw new ServiceException($"Token failed validation: {stvex.Message}");
            }
            catch (ArgumentException argex)
            {
                throw new ServiceException($"Token was invalid: {argex.Message}");
            }
        
            var userIdClaim = jwt.Claims.FirstOrDefault(claim => claim.Type == "uid");

            if (userIdClaim == null)
            {
                throw new InvalidModelException("Invalid token");
            }

            var actionIdClaim = jwt.Claims.FirstOrDefault(claim => claim.Type == "aid");

            if (actionIdClaim == null)
            {
                throw new InvalidModelException("Invalid token");
            }

            Guid userId   = Guid.Parse(userIdClaim.Value);
            Guid actionId = Guid.Parse(actionIdClaim.Value);

            lock (syncTokenObj)
            {
                bool hasToken = tokens.Any(t => t.actionId.Equals(actionId) && t.userId.Equals(userId) && t.type == TokenType.MailConfirmation);

                if (!hasToken)
                {
                    throw new InvalidModelException("Invalid token");
                }

                tokens.Remove(tokens.First(t => t.actionId.Equals(actionId) && t.userId.Equals(userId)));
            }

            var user = await _context.Set<Entities.User>()
                                     .Include(usr => usr.UserInfo)
                                     .FirstOrDefaultAsync(usr => usr.Id.Equals(userId));

            user.UserInfo.EmailConfirmed = true;

            await _context.SaveChangesAsync();

#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
            Task.Run(CheckAllTokens);
#pragma warning restore CS4014
        }



        public bool CheckSession(Guid userId, Guid sessionId)
        {
            lock (syncSessionObj)
            {
                return sessions.Any(s => s.sessionId.Equals(sessionId) && s.userId.Equals(userId));
            }
        }

        public void SignOut(Guid userId)
        {
            lock (syncSessionObj)
            {
                var userSession = sessions.FirstOrDefault(s => s.userId.Equals(userId));
                sessions.Remove(userSession);
            }
        }

        public async Task<Dtos.TokenInfo> RefreshTokenAsync(string refreshToken)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer              = _appSettings.Issuer,
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidAudience            = _appSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret)),
                RequireExpirationTime    = true,
                ValidateLifetime         = true
            };

            JwtSecurityToken jwt;

            try
            {
                var claimsPrincipal = new JwtSecurityTokenHandler()
                    .ValidateToken(refreshToken, validationParameters, out var rawValidatedToken);

                jwt = (JwtSecurityToken)rawValidatedToken;
            }
            catch (SecurityTokenValidationException stvex)
            {
                throw new ServiceException($"Token failed validation: {stvex.Message}");
            }
            catch (ArgumentException argex)
            {
                throw new ServiceException($"Token was invalid: {argex.Message}");
            }
        
            var refreshTokenIdClaim = jwt.Claims.FirstOrDefault(claim => claim.Type == "rid");

            if (refreshTokenIdClaim == null)
            {
                throw new InvalidModelException("Invalid token");
            }

            var sessionIdClaim = jwt.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sid);

            if (sessionIdClaim == null)
            {
                throw new InvalidModelException("Invalid token");
            }

            Guid refreshTokenId = Guid.Parse(refreshTokenIdClaim.Value);
            Guid sessionId      = Guid.Parse(sessionIdClaim.Value);

            Monitor.Enter(syncSessionObj);

            var hasSession = sessions.Any(s => s.sessionId.Equals(sessionId) && s.refreshTokenId.Equals(refreshTokenId));

            if (hasSession)
            {
                SessionInfo session = sessions.First(s => s.sessionId.Equals(sessionId) && s.refreshTokenId.Equals(refreshTokenId));

                sessions.Remove(session);

                Monitor.Exit(syncSessionObj);

                Entities.User user = await _context.Set<Entities.User>()
                                                   .Include(usr => usr.UserRoles)
                                                   .ThenInclude(ur => ur.Role)
                                                   .FirstOrDefaultAsync(usr => usr.Id == session.userId);

                this.CreateTokens(user, out string newAccessToken, out string newRefreshToken);

                return new Dtos.TokenInfo
                {
                    AccessToken  = "Bearer " + newAccessToken,
                    RefreshToken = newRefreshToken,
                    Expire       = _appSettings.AccessTokenLifeTime
                };
            }
            else
            {
                Monitor.Exit(syncSessionObj);
                throw new NotFoundException("Session not found");
            }
        }


        #region CRUD
        public async Task<IEnumerable<Dtos.User>> GetAllAsync()
        {
            return await _context.Set<Entities.User>()
                                 .Include(x => x.UserInfo)
                                 .Include(x => x.UserRoles)
                                 .ThenInclude(ur => ur.Role)
                                 .ThenInclude(r => r.RolePermissions)
                                 .ThenInclude(rp => rp.Permission)
                                 .Where(user => !user.UserRoles.GroupBy(ur => ur.Role)
                                                               .Select(r => r.Key.Name)
                                                               .Contains(AppRoles.SuperAdmin))
                                 .AsNoTracking()
                                 .Select(user => new Dtos.User
                                 {
                                     CreatedAt    = user.CreatedAt,
                                     DateOfBirth  = user.UserInfo.DateOfBirth,
                                     Email        = user.UserInfo.Email,
                                     FirstName    = user.UserInfo.FirstName,
                                     FirstNameEng = user.UserInfo.FirstNameEng,
                                     Id           = user.Id,
                                     LastName     = user.UserInfo.LastName,
                                     LastNameEng  = user.UserInfo.LastNameEng,
                                     Patronymic   = user.UserInfo.Patronymic,
                                     PhoneNumber  = user.UserInfo.PhoneNumber,
                                     UpdatedAt    = user.UpdatedAt,
                                     Roles        = user.UserRoles.GroupBy(x => x.Role)
                                                                  .Select(r => new Dtos.Role
                                                                  {
                                                                      Id          = r.Key.Id,
                                                                      Name        = r.Key.Name,
                                                                      Permissions = r.Key.RolePermissions.GroupBy(rp => rp.Permission)
                                                                                                         .Select(perm => new Dtos.Permission 
                                                                                                         {
                                                                                                             Id = perm.Key.Id,
                                                                                                             AccessModifier = perm.Key.AccessModifier,
                                                                                                             TargetModifier = perm.Key.TargetModifier
                                                                                                         })
                                             }),                  
                                 })
                                 .OrderBy(s => s.FirstName)
                                 .ThenBy(s => s.LastName)
                                 .ThenBy(s => s.Patronymic)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.User>> GetAllAsync(int page, int size)
        {
            int usersToSkip = page * size;

            return await _context.Set<Entities.User>()
                                 .Include(x => x.UserInfo)
                                 .Include(x => x.UserRoles)
                                 .ThenInclude(ur => ur.Role)
                                 .ThenInclude(r => r.RolePermissions)
                                 .ThenInclude(rp => rp.Permission)
                                 .Skip(usersToSkip)
                                 .Take(size)
                                 .Where(user => !user.UserRoles.GroupBy(ur => ur.Role)
                                                               .Select(r => r.Key.Name)
                                                               .Contains(AppRoles.SuperAdmin))
                                 .AsNoTracking()
                                 .Select(user => new Dtos.User
                                 {
                                     CreatedAt    = user.CreatedAt,
                                     DateOfBirth  = user.UserInfo.DateOfBirth,
                                     Email        = user.UserInfo.Email,
                                     FirstName    = user.UserInfo.FirstName,
                                     FirstNameEng = user.UserInfo.FirstNameEng,
                                     Id           = user.Id,
                                     LastName     = user.UserInfo.LastName,
                                     LastNameEng  = user.UserInfo.LastNameEng,
                                     Patronymic   = user.UserInfo.Patronymic,
                                     PhoneNumber  = user.UserInfo.PhoneNumber,
                                     UpdatedAt    = user.UpdatedAt,
                                     Roles = user.UserRoles.GroupBy(x => x.Role)
                                                 .Select(r => new Dtos.Role
                                                 {
                                                     Id = r.Key.Id,
                                                     Name = r.Key.Name,
                                                     Permissions = r.Key.RolePermissions.GroupBy(rp => rp.Permission)
                                                                                        .Select(perm => new Dtos.Permission
                                                                                        {
                                                                                            Id = perm.Key.Id,
                                                                                            AccessModifier = perm.Key.AccessModifier,
                                                                                            TargetModifier = perm.Key.TargetModifier
                                                                                        })
                                                 }),
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.User> GetByIdAsync(Guid id)
        {
            var user = await _context.Set<Entities.User>()
                                     .Include(x => x.UserInfo)
                                     .Include(x => x.UserRoles)
                                     .ThenInclude(ur => ur.Role)
                                     .ThenInclude(r => r.RolePermissions)
                                     .ThenInclude(rp => rp.Permission)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return null;

            return new Dtos.User
            {
                CreatedAt    = user.CreatedAt,
                DateOfBirth  = user.UserInfo.DateOfBirth,
                Email        = user.UserInfo.Email,
                FirstName    = user.UserInfo.FirstName,
                FirstNameEng = user.UserInfo.FirstNameEng,
                Id           = user.Id,
                LastName     = user.UserInfo.LastName,
                LastNameEng  = user.UserInfo.LastNameEng,
                Patronymic   = user.UserInfo.Patronymic,
                PhoneNumber  = user.UserInfo.PhoneNumber,
                UpdatedAt    = user.UpdatedAt,
                Roles        = user.UserRoles.GroupBy(x => x.Role)
                                             .Select(r => new Dtos.Role
                                             {
                                                 Id = r.Key.Id,
                                                 Name = r.Key.Name,
                                                 Permissions = r.Key.RolePermissions.GroupBy(rp => rp.Permission)
                                                                                    .Select(perm => new Dtos.Permission
                                                                                    {
                                                                                        Id = perm.Key.Id,
                                                                                        AccessModifier = perm.Key.AccessModifier,
                                                                                        TargetModifier = perm.Key.TargetModifier
                                                                                    })
                                             }),
            };
        }

        public async Task<SieveResult<Dtos.User>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var usersQuery = _context.Set<Entities.User>()
                                     .Include(x => x.UserInfo)
                                     .Include(x => x.UserRoles)
                                     .ThenInclude(ur => ur.Role)
                                     .ThenInclude(r => r.RolePermissions)
                                     .ThenInclude(rp => rp.Permission)
                                     .Where(user => !user.UserRoles.GroupBy(ur => ur.Role)
                                                               .Select(r => r.Key.Name)
                                                               .Contains(AppRoles.SuperAdmin))
                                     .AsNoTracking();

            usersQuery = _sieveProcessor.Apply(model, usersQuery, applyPagination: false);

            var result = new SieveResult<Dtos.User>();
            result.TotalCount = await usersQuery.CountAsync();

            var someUsers = await _sieveProcessor.Apply(model, usersQuery, applyFiltering: false, applySorting: false).ToListAsync();

            result.Result = someUsers.Select(user => new Dtos.User
            {
                CreatedAt    = user.CreatedAt,
                DateOfBirth  = user.UserInfo.DateOfBirth,
                Email        = user.UserInfo.Email,
                FirstName    = user.UserInfo.FirstName,
                FirstNameEng = user.UserInfo.FirstNameEng,
                Id           = user.Id,
                LastName     = user.UserInfo.LastName,
                LastNameEng  = user.UserInfo.LastNameEng,
                Patronymic   = user.UserInfo.Patronymic,
                PhoneNumber  = user.UserInfo.PhoneNumber,
                UpdatedAt    = user.UpdatedAt,
                Roles        = user.UserRoles.GroupBy(x => x.Role)
                                             .Select(r => new Dtos.Role
                                             {
                                                 Id = r.Key.Id,
                                                 Name = r.Key.Name,
                                                 Permissions = r.Key.RolePermissions.GroupBy(rp => rp.Permission)
                                                                                    .Select(perm => new Dtos.Permission
                                                                                    {
                                                                                        Id = perm.Key.Id,
                                                                                        AccessModifier = perm.Key.AccessModifier,
                                                                                        TargetModifier = perm.Key.TargetModifier
                                                                                    })
                                             }),
            });

            return result;
        }

        public async Task UpdateAsync(Dtos.User dto)
        {
            if (dto == null)
                throw new NotFoundException("Target user not found");

            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.UserValidator();
            ValidationResult result = validator.Validate(dto);

            if (!result.IsValid)
            {
                string errMess = string.Empty;

                foreach (var failure in result.Errors)
                {
                    errMess += $"Property { failure.PropertyName } failed validation. Error was: { failure.ErrorMessage }\n";
                }

                throw new InvalidModelException(errMess);
            }

            var targetUser = await _context.Set<Entities.User>()
                                           .Include(x => x.UserInfo)
                                           .FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (targetUser == null)
                throw new NotFoundException("Target user not found");

            targetUser.UserInfo.FirstName    = dto.FirstName;
            targetUser.UserInfo.LastName     = dto.LastName;
            targetUser.UserInfo.Patronymic   = dto.Patronymic;
            targetUser.UserInfo.FirstNameEng = dto.FirstNameEng;
            targetUser.UserInfo.LastNameEng  = dto.LastNameEng;
            targetUser.UserInfo.Email        = dto.Email;
            targetUser.UserInfo.PhoneNumber  = dto.PhoneNumber;
            targetUser.UserInfo.DateOfBirth  = dto.DateOfBirth;
            targetUser.UpdatedAt             = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var targetUser = await _context.Set<Entities.User>()
                                           .FirstOrDefaultAsync(x => x.Id == id);
            if (targetUser == null)
                throw new NotFoundException("Target user not found");

            var userInfoId = targetUser.UserInfoId;

            _context.Set<Entities.UserRoles>().RemoveRange(targetUser.UserRoles);
            _context.Remove(targetUser);
            await _context.SaveChangesAsync();

            await _userInfoService.RemoveAsync(userInfoId);
        }


        public async Task AddUserToRoleAsync(Guid userId, Guid roleId)
        {
            var user = await _context.Set<Entities.User>().FindAsync(userId);

            if (user == null)
                throw new NotFoundException("User is not found");

            var role = await _context.Set<Entities.Role>().FindAsync(roleId);

            if (role == null)
                throw new NotFoundException("Role is not found");

            var userRole = await _context.Set<Entities.UserRoles>().FirstOrDefaultAsync(ur => ur.RoleId == role.Id && ur.UserId == user.Id);

            if (userRole != null)
                return;

            await _context.Set<Entities.UserRoles>().AddAsync(
                new Entities.UserRoles
                {
                    RoleId = role.Id,
                    UserId = user.Id
                }
                );

            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromRoleAsync(Guid userId, Guid roleId)
        {
            var user = await _context.Set<Entities.User>().FindAsync(userId);

            if (user == null)
                throw new NotFoundException("User is not found");

            var role = await _context.Set<Entities.Role>().FindAsync(roleId);

            if (role == null)
                throw new NotFoundException("Role is not found");

            var userRole = await _context.Set<Entities.UserRoles>().FirstOrDefaultAsync(ur => ur.RoleId == role.Id && ur.UserId == user.Id);

            if (userRole == null)
                return;

            _context.Remove(userRole);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasRoleAsync(Guid userId, Guid roleId)
        {
            return await _context.Set<Entities.UserRoles>()
                                 .AnyAsync(ur => ur.RoleId == roleId && ur.UserId == userId);
        }

        public async Task<IEnumerable<Dtos.Role>> GetUserRolesAsync(Guid userId)
        {
            bool isSuperAdmin = await _context.Set<Entities.UserRoles>()
                                              .Include(ur => ur.Role)
                                              .AnyAsync(ur => ur.Role.Name == AppRoles.SuperAdmin && ur.UserId == userId);

            if (isSuperAdmin)
            {
                throw new NotFoundException("User not found");
            }

            return await _context.Set<Entities.UserRoles>()
                                 .Include(ur => ur.Role)
                                 .ThenInclude(r => r.RolePermissions)
                                 .ThenInclude(rp => rp.Permission)
                                 .Where(userRole => userRole.UserId == userId)
                                 .AsNoTracking()
                                 .Select(ur => new Dtos.Role
                                 {
                                     CreatedAt   = ur.Role.CreatedAt,
                                     Id          = ur.Role.Id,
                                     UpdatedAt   = ur.Role.UpdatedAt,
                                     Name        = ur.Role.Name,
                                     Permissions = ur.Role.RolePermissions
                                                          .GroupBy(x => x.Permission)
                                                          .Select(p => new Dtos.Permission
                                                          {
                                                              Id             = p.Key.Id,
                                                              AccessModifier = p.Key.AccessModifier,
                                                              TargetModifier = p.Key.TargetModifier
                                                          }
                                                          )
                                 }
                                 )
                                 .ToListAsync();
        }

        public async Task<bool> HasPermissionAsync(Guid userId, AccessModifier accessModifier, TargetModifier targetModifier)
        {
            bool isSuperAdmin = await _context.Set<Entities.UserRoles>()
                                              .Include(ur => ur.Role)
                                              .AnyAsync(ur => ur.Role.Name == AppRoles.SuperAdmin && ur.UserId == userId);

            if (isSuperAdmin)
            {
                return true;
            }
            
            return await _context.Set<Entities.UserRoles>()
                                 .Include(ur => ur.Role)
                                 .ThenInclude(r => r.RolePermissions)
                                 .ThenInclude(rp => rp.Permission)
                                 .AnyAsync(userRole => userRole.UserId == userId &&
                                                       userRole.Role.RolePermissions.Any(rp =>
                                                           rp.Permission.AccessModifier.HasFlag(accessModifier) && rp.Permission.TargetModifier == targetModifier
                                                       ));
        }
        #endregion

        #region Helpers
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");

            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void CreateTokens(Entities.User user, out string accessToken, out string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);

            var claimes = new List<Claim>();

            Guid sessionId = Guid.NewGuid();

            claimes.Add(new Claim(ClaimTypes.Name, user.Id.ToString()));
            claimes.Add(new Claim(JwtRegisteredClaimNames.Sid, sessionId.ToString()));

            foreach (var role in user.UserRoles.ToList())
            {
                claimes.Add(new Claim(ClaimTypes.Role, role.Role.Name));
            }

            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject            = new ClaimsIdentity(claimes),
                Expires            = DateTime.UtcNow.AddMinutes(_appSettings.AccessTokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience           = _appSettings.Audience,
                Issuer             = _appSettings.Issuer
            };

            var acsToken = tokenHandler.CreateToken(accessTokenDescriptor);
            accessToken = tokenHandler.WriteToken(acsToken);


            // creating refresh token
            Guid refreshTId = Guid.NewGuid();

            claimes = new List<Claim>();
            claimes.Add(new Claim(JwtRegisteredClaimNames.Sid, sessionId.ToString()));
            claimes.Add(new Claim("rid", refreshTId.ToString()));

            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject            = new ClaimsIdentity(claimes),
                Expires            = DateTime.UtcNow.AddMinutes(_appSettings.RefreshTokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience           = _appSettings.Audience,
                Issuer             = _appSettings.Issuer
            };

            var refToken = tokenHandler.CreateToken(refreshTokenDescriptor);
            refreshToken = tokenHandler.WriteToken(refToken);
            
            // register token
            sessions.Add(
                new SessionInfo
                {
                    sessionId    = sessionId,
                    expires      = DateTime.UtcNow.AddMinutes(_appSettings.RefreshTokenLifeTime),
                    userId       = user.Id,
                    refreshTokenId = refreshTId
                }
                );
        }

        private static void CheckAllSessions()
        {
            lock (syncSessionObj)
            {
                for (int i = 0; i < sessions.Count; ++i)
                {
                    if (sessions[i].expires < DateTime.UtcNow)
                    {
                        sessions.Remove(sessions[i]);
                    }
                }
            }
        }

        private static void CheckAllTokens()
        {
            lock (syncTokenObj)
            {
                for (int i = 0; i < tokens.Count; ++i)
                {
                    if (tokens[i].expires < DateTime.UtcNow)
                    {
                        tokens.Remove(tokens[i]);
                    }
                }
            }
        }

        public Task ResetPasswordAsync(string token, string newPassword)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
