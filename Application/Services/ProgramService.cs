using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;

namespace Application.Services
{
    public class ProgramService : IProgramService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProgramService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<ProgramDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Program.GetAll(new ProgramSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<ProgramDto>>(_mapper.Map<List<ProgramDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Program.TotalRecord(new ProgramSpecs(filter, true));
            return new PaginationResponse<List<ProgramDto>>(_mapper.Map<List<ProgramDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<ProgramDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Program.GetById(id, new ProgramSpecs());
            if (result == null)
                return new Response<ProgramDto>("Not found");
            var semesterCourseList = new List<SemesterCousesDto>();
            var getProgramSemesters = await _unitOfWork.Program.GetProgramSemesters(id);
            if (getProgramSemesters.Any())
            {
                foreach (var item in getProgramSemesters)
                {
                    var getProgramCourses = await _unitOfWork.Program.GetCourses(item.Id);
                    foreach (var programCourses in getProgramCourses)
                    {
                        semesterCourseList.Add(new SemesterCousesDto()
                        {
                            CourseId = programCourses.CourseId,
                            Course = programCourses.Course.Name,
                            SemesterId = item.SemesterId,
                            Semester = item.Semester.Name,
                        });
                    }   
                }
            }
            var programDto = _mapper.Map<ProgramDto>(result);
            programDto.SemesterCoursesList = semesterCourseList;
            return new Response<ProgramDto>(programDto, "Returning value");
        }

        public async Task<Response<List<ProgramDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Program.GetAll();
            if (!result.Any())
                return new Response<List<ProgramDto>>(null, "List is empty");

            return new Response<List<ProgramDto>>(_mapper.Map<List<ProgramDto>>(result), "Returning List");
        }

        public async Task<Response<List<ProgramSemesterDto>>> GetProgramSemesters(int programId)
        {
            var result = await _unitOfWork.Program.GetProgramSemesters(programId);
            if (!result.Any())
                return new Response<List<ProgramSemesterDto>>(null, "List is empty");

            return new Response<List<ProgramSemesterDto>>(_mapper.Map<List<ProgramSemesterDto>>(result), "Returning List");
        }

        public async Task<Response<ProgramDto>> CreateAsync(CreateProgramDto entity)
        {
            //Checking duplicate courses if any
            var duplicates = entity.SemesterCousesList.GroupBy(x => new { x.CourseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();
            if (duplicates.Any())
                return new Response<ProgramDto>("Courses must be unique for each semester");

            //creating transaction
            _unitOfWork.CreateTransaction();

            //Adding program
            var program = await _unitOfWork.Program.Add(_mapper.Map<Program>(entity));
            await _unitOfWork.SaveAsync();

            //Getting unique semester
            var semesters = entity.SemesterCousesList
                .Select(i => i.SemesterId)
                .Distinct().ToList();

            //Adding semester in program semester table
            var programSemesterList = new List<ProgramSemester>();
            foreach (var semester in semesters)
            {
                programSemesterList.Add(new ProgramSemester(program.Id, (int)semester));
            }
            var programSemester = _unitOfWork.Program.AddProgramSemesters(programSemesterList);
            await _unitOfWork.SaveAsync();

            //getting program semesters
            var getProgramSemesters = await _unitOfWork.Program.GetProgramSemesters(program.Id);

            var programCourses = new List<ProgramCourse>();
            foreach (var item in entity.SemesterCousesList)
            {
                int programSemesterId = getProgramSemesters
                    .Where(i => i.ProgramId == program.Id && i.SemesterId == item.SemesterId)
                    .Select(i => i.Id)
                    .FirstOrDefault();
                programCourses.Add(new ProgramCourse(programSemesterId, (int)item.CourseId));
            }
            await _unitOfWork.Program.AddProgramCourses(programCourses);
            await _unitOfWork.SaveAsync();
            _unitOfWork.Commit();

            return new Response<ProgramDto>(_mapper.Map<ProgramDto>(program), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<ProgramDto>> UpdateAsync(CreateProgramDto entity)
        {
            //Checking duplicate courses if any
            var duplicates = entity.SemesterCousesList.GroupBy(x => new { x.CourseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();
            if (duplicates.Any())
                return new Response<ProgramDto>("Courses must be unique for each semester");

            var result = await _unitOfWork.Program.GetById((int)entity.Id);
            if (result == null)
                return new Response<ProgramDto>("Not found");

            //creating transaction
            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();

            //Removing program courses
            await _unitOfWork.Program.RemoveProgramCouses(result.Id);
            await _unitOfWork.SaveAsync();
            
            //Getting unique semester
            var semesters = entity.SemesterCousesList
                .Select(i => i.SemesterId)
                .Distinct().ToList();

            //Adding semester in program semester table
            var programSemesterList = new List<ProgramSemester>();
            foreach (var semester in semesters)
            {
                programSemesterList.Add(new ProgramSemester(result.Id, (int)semester));
            }
            var programSemester = _unitOfWork.Program.AddProgramSemesters(programSemesterList);
            await _unitOfWork.SaveAsync();

            //getting program semesters
            var getProgramSemesters = await _unitOfWork.Program.GetProgramSemesters(result.Id);

            var programCourses = new List<ProgramCourse>();
            foreach (var item in entity.SemesterCousesList)
            {
                int programSemesterId = getProgramSemesters
                    .Where(i => i.ProgramId == result.Id && i.SemesterId == item.SemesterId)
                    .Select(i => i.Id)
                    .FirstOrDefault();
                programCourses.Add(new ProgramCourse(programSemesterId, (int)item.CourseId));
            }
            await _unitOfWork.Program.AddProgramCourses(programCourses);
            await _unitOfWork.SaveAsync();

            _unitOfWork.Commit();
            return new Response<ProgramDto>(_mapper.Map<ProgramDto>(result), "Updated successfully");
        }

        public async Task<Response<int>> AddFees(List<AddSemesterFeesDto> entity)
        {
            var programFees = new List<ProgramFees>();
            foreach (var item in entity)
            {
                programFees.Add(new ProgramFees((int)item.ProgramSemesterId,
                    (int)item.FeeItemId, (decimal)item.Amount));
            }
            await _unitOfWork.Program.AddProgramFees(programFees);
            await _unitOfWork.SaveAsync();
            return new Response<int>(1, "Added successfully");
        }

    }
}
