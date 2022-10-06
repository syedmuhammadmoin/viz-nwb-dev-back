using Application.Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Response
{
    public class Response<T>
    {
        private FileStreamResult filess;

        public T Result { get; protected set; }
        public bool IsSuccess { get; protected set; }
        public string[] Errors { get; protected set; }
        public string Message { get; protected set; }
        protected Response()
        {
        }

        public Response(T data, string message)
        {
            IsSuccess = true;
            Message = message;
            Errors = null;
            Result = data;
        }
        public Response(T data, string message, string[] errors)
        {
            IsSuccess = true;
            Message = message;
            Errors = errors;
            Result = data;
        }

        public Response(string message)
        {
            IsSuccess = false;
            Message = message;
        }

        public Response(FileStreamResult filess)
        {
            this.filess = filess;
        }
    }
}
