using eUniversityServer.DAL.Enums;
using eUniversityServer.Services.Dtos;
using eUniversityServer.Services.Models;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserWithToken> SignInAsync(string email, string password);

        Task<Guid> SignUpAsync(User dto, string password);


        Task SendPasswordRecoveryEmailAsync(string url, string email);

        Task ResetPasswordAsync(string token, string newPassword);


        bool CheckSession(Guid userId, Guid sessionId);

        void SignOut(Guid userId);

        Task<Dtos.TokenInfo> RefreshTokenAsync(string token);


        Task SendConfirmationEmailAsync(string url, string email);

        Task ConfirmEmailAsync(string token);


        Task<IEnumerable<User>> GetAllAsync();

        Task<IEnumerable<User>> GetAllAsync(int page, int size);

        Task<SieveResult<User>> GetSomeAsync(SieveModel model);

        Task<User> GetByIdAsync(Guid id);

        Task UpdateAsync(User dto);

        Task RemoveAsync(Guid id);

        Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId);

        Task AddUserToRoleAsync(Guid userId, Guid roleId);

        Task RemoveUserFromRoleAsync(Guid userId, Guid roleId);

        Task<bool> HasRoleAsync(Guid userId, Guid roleId);

        Task<bool> HasPermissionAsync(Guid userId, AccessModifier accessModifier, TargetModifier targetModifier);
    }
}
