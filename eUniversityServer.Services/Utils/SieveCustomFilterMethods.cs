using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities = eUniversityServer.DAL.Entities;

namespace eUniversityServer.Services.Utils
{
    public class SieveCustomFilterMethods : ISieveCustomFilterMethods
    {
        public IQueryable<Entities.Student> FullName(IQueryable<Entities.Student> students, string op, string[] values)
        {
            if (values.Length <= 0)
            {
                return students;
            }

            IQueryable<Entities.Student> result = students;

            switch (op)
            {
                case "!@=*":
                    result = students.Include(s => s.UserInfo)
                                     .Where(s => !(s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).ToLower().Contains(values[0].ToLower()));
                    break;
                case "!_=*":
                    result = students.Include(s => s.UserInfo)
                                     .Where(s => !(s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).ToLower().StartsWith(values[0].ToLower()));
                    break;
                case "!=*":
                    result = students.Include(s => s.UserInfo)
                                     .Where(s => (s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).ToLower() != values[0].ToLower());
                    break;
                case "!@=":
                    result = students.Include(s => s.UserInfo)
                                     .Where(s => !(s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).Contains(values[0]));
                    break;
                case "!_=":
                    result = students.Include(s => s.UserInfo)
                                     .Where(s => !(s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).StartsWith(values[0]));
                    break;
                case "==*":
                    result = students.Include(s => s.UserInfo)
                                     .Where(s => (s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).ToLower() == values[0].ToLower());
                    break;
                case "@=*":
                    result = students.Include(s => s.UserInfo)
                                     .Where(s => (s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).ToLower().Contains(values[0].ToLower()));
                    break;
                case "_=*":
                    result = students.Include(s => s.UserInfo)
                                     .Where(s => (s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).ToLower().StartsWith(values[0].ToLower()));
                    break;
                case "==":
                    result = students.Include(s => s.UserInfo)
                                     .Where(s => s.UserInfo.FirstName + ' ' + s.UserInfo.LastName == values[0]);
                    break;
                case "!=":
                    result = students.Include(s => s.UserInfo)
                                     .Where(s => s.UserInfo.FirstName + ' ' + s.UserInfo.LastName != values[0]);
                    break;
                case "@=":
                    result = students.Include(s => s.UserInfo)
                                     .Where(s => (s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).Contains(values[0])); 
                    break;
                case "_=":
                    result = students.Include(s => s.UserInfo)
                                     .Where(s => (s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).StartsWith(values[0]));
                    break;
                default: 
                    return students;
            }

            return result;
        }

        public IQueryable<Entities.Teacher> FullName(IQueryable<Entities.Teacher> teachers, string op, string[] values)
        {
            if (values.Length <= 0)
            {
                return teachers;
            }

            IQueryable<Entities.Teacher> result = teachers;

            switch (op)
            {
                case "!@=*":
                    result = teachers.Include(s => s.UserInfo)
                                     .Where(s => !(s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).ToLower().Contains(values[0].ToLower()));
                    break;
                case "!_=*":
                    result = teachers.Include(s => s.UserInfo)
                                     .Where(s => !(s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).ToLower().StartsWith(values[0].ToLower()));
                    break;
                case "!=*":
                    result = teachers.Include(s => s.UserInfo)
                                     .Where(s => (s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).ToLower() != values[0].ToLower());
                    break;
                case "!@=":
                    result = teachers.Include(s => s.UserInfo)
                                     .Where(s => !(s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).Contains(values[0]));
                    break;
                case "!_=":
                    result = teachers.Include(s => s.UserInfo)
                                     .Where(s => !(s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).StartsWith(values[0]));
                    break;
                case "==*":
                    result = teachers.Include(s => s.UserInfo)
                                     .Where(s => (s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).ToLower() == values[0].ToLower());
                    break;
                case "@=*":
                    result = teachers.Include(s => s.UserInfo)
                                     .Where(s => (s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).ToLower().Contains(values[0].ToLower()));
                    break;
                case "_=*":
                    result = teachers.Include(s => s.UserInfo)
                                     .Where(s => (s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).ToLower().StartsWith(values[0].ToLower()));
                    break;
                case "==":
                    result = teachers.Include(s => s.UserInfo)
                                     .Where(s => s.UserInfo.FirstName + ' ' + s.UserInfo.LastName == values[0]);
                    break;
                case "!=":
                    result = teachers.Include(s => s.UserInfo)
                                     .Where(s => s.UserInfo.FirstName + ' ' + s.UserInfo.LastName != values[0]);
                    break;
                case "@=":
                    result = teachers.Include(s => s.UserInfo)
                                     .Where(s => (s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).Contains(values[0]));
                    break;
                case "_=":
                    result = teachers.Include(s => s.UserInfo)
                                     .Where(s => (s.UserInfo.FirstName + ' ' + s.UserInfo.LastName).StartsWith(values[0]));
                    break;
                default:
                    return teachers;
            }

            return result;
        }
    }
}
