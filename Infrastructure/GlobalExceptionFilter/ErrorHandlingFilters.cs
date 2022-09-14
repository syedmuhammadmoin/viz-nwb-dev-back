using Application.Contracts.DTOs;
using Application.Contracts.Response;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.GlobalExceptionFilter
{
    public class ErrorHandlingFilters : ExceptionFilterAttribute
    {
        private readonly ApplicationDbContext _dbContext;
        public ErrorHandlingFilters(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var StatusCode = StatusCodes.Status500InternalServerError;
            string id = context.HttpContext.TraceIdentifier;
            var StackTrace = exception.StackTrace;
            var result = new Response<LogItemDto>("Something went wrong");
            var IsSuccess = result.IsSuccess;
            var Errors = result.Errors;
            var res = result.Result;
            var Message = result.Message;
            context.Result = new ObjectResult(new { Message = Message, TraceId = id, StasCode = StatusCode, Result = res, IsSuccess = IsSuccess, Errors = Errors })
            {
                StatusCode = 500
            };
            var Log = new LogItem
            {
                Status = StatusCode,
                Title = "Something Went Wrong",
                Detail = StackTrace,
                TraceId = id
            };
            _dbContext.LogItems.Add(Log);
            _dbContext.SaveChanges();
            context.ExceptionHandled = true;

        }

    }
}
