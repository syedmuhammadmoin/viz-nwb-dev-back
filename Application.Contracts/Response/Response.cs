﻿using Application.Contracts.DTOs;
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
        public T Result { get; protected set; }
        public bool IsSuccess { get; protected set; }
        public string Message { get; protected set; }
        public int StatusCode { get; protected set; }
        public string TraceId { get; protected set; }
        public Object Errors { get;  set; }


        protected Response()
        {
        }

        public Response(T data, string message)
        {
            IsSuccess = true;
            Message = message;
            StatusCode = 200;
            Result = data;
        }
        public Response(T data, string message, string[] errors)
        {
            IsSuccess = true;
            Message = message;
            StatusCode = 200;
            Errors = errors;
            Result = data;
        }

        public Response(string message)
        {
            IsSuccess = false;
            Message = message;
            StatusCode = 400;
        }
        public Response(string message, int statusCode, string traceId) : this(message)
        {
            IsSuccess = false;
            Message = message;
            StatusCode = statusCode;
            TraceId = traceId;
      
        }
        public Response(string message, int statusCode , string traceId ,Object Error) : this(message)
        {   
            IsSuccess = false;
            Message = message;
            StatusCode = statusCode;
            TraceId = traceId;
            Errors = Error;
        }
    }
}
