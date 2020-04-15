using System;
using System.Linq;
using System.Threading.Tasks;
using eUniversityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace eUniversityServer.Services
{
    internal sealed class UserInfoService
    {
        private readonly DbContext context;

        public UserInfoService(DbContext context)
        {
            this.context = context ?? throw new NullReferenceException(nameof(context));
        }
        
        internal async Task RemoveAsync(Guid userInfoId)
        {
            var userInfo = await context.Set<UserInfo>()
                                        .FindAsync(userInfoId);

            if (userInfo == null)
                return;
            
            var studentHas = await context.Set<Student>()
                                          .AnyAsync(st => st.UserInfoId == userInfoId);

            if (studentHas)
                return;
            
            var userHas = await context.Set<Student>()
                                       .AnyAsync(st => st.UserInfoId == userInfoId);
            
            if (userHas)
                return;
            
            var teacherHas = await context.Set<Student>()
                                          .AnyAsync(st => st.UserInfoId == userInfoId);
            
            if (teacherHas)
                return;
            
            context.Remove(userInfo);
            await context.SaveChangesAsync();
        }
    }
}