
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
            var responses = new Response<bool>("Something went wrong", StatusCodes.Status500InternalServerError, context.HttpContext.TraceIdentifier);
            context.Result = new ObjectResult(responses)
            {
                StatusCode = 500
            };
            var Logs = new LogItem
            {
                Status = responses.StatusCode,
                Message = exception.Message,
                Detail = exception.StackTrace,
                TraceId = context.HttpContext.TraceIdentifier
            };
            _dbContext.LogItems.Add(Logs);
            _dbContext.SaveChanges();
            context.ExceptionHandled = true;
      
        }

    }
}
