using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface ICourseService : ICrudService<CourseDto, CourseDto, int, TransactionFormFilter>
    {
        Task<Response<List<CourseDto>>> GetDropDown();
    }
}
