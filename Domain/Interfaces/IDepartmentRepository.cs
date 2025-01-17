﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department, int>
    {
        Task AddRange(List<Department> list);
    }
}
