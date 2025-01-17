﻿
using Application.Contracts.Response;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.GlobalExceptionFilter
{
    public class ErrorHandlingFilters : ExceptionFilterAttribute
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ErrorHandlingFilters(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

        }
        public override void OnException(ExceptionContext context)
        {

            //RollBack Previous Transaction from Service
            _unitOfWork.Rollback();
            var exception = context.Exception;
            var responses = new Response<bool>($"Something went wrong {context.HttpContext.TraceIdentifier}", StatusCodes.Status500InternalServerError, context.HttpContext.TraceIdentifier, context.Exception);

            // commit in Development Mode Only
            context.Result = new ObjectResult(responses)
            {
                StatusCode = 500
            };

            string result;
            if (exception.InnerException == null)
            {
                result = exception.StackTrace;
            }
            else
            {
                result = exception.InnerException.Message;
            }

            var Logs = new LogItem
            {
                Status = responses.StatusCode,
                Message = exception.Message,
                Detail = result,
                TraceId = context.HttpContext.TraceIdentifier,
            };

            // Creating New Context
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
           .Options;

            using (var context2 = new ApplicationDbContext(options, _httpContextAccessor))
            {
                // Creating New Transaction
                var transaction = context2.Database.BeginTransaction();
                context2.Database.UseTransaction(transaction.GetDbTransaction());
                context2.LogItems.Add(Logs);
                context2.SaveChanges();
                transaction.Commit();
            }

            //append Logs.Id to exception message

            // false for Development purpose only
            context.ExceptionHandled = true;
        }
    }
}
