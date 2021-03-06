﻿using eUniversityServer.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Interfaces
{
    public interface ITeacherService : IServiceMoreInfo<Teacher, Dtos.TeacherInfo>
    {
    }
}
