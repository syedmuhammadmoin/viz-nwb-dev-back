using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface ILevel3Service 
    {
        Task<Response<List<Level3DropDownDto>>> GetLevel3DropDown();
    }
}
