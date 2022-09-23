using Application.Contracts.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.GlobalExceptionFilter
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(HttpContext context, ModelStateDictionary modelState)
            : base(new Response<bool>( "Validation Failed" , 400 , context.TraceIdentifier, modelState.Keys
                     .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, 0, x.ErrorMessage)))
                     .ToList()))
        { 
            StatusCode = StatusCodes.Status400BadRequest; //change the http status code to 422.
        }
    }
}
