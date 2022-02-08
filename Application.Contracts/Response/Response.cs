using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Response
{
    public class Response<T>
    {
        private Response()
        {
        }

        public Response(T data, string message)
        {
            IsSuccess = true;
            Message = message;
            Errors = null;
            Result = data;
        }

        public Response(string message)
        {
            IsSuccess = false;
            Message = message;
        }

        public T Result { get; private set; }
        public bool IsSuccess { get; private set; }
        public string[] Errors { get; private set; }
        public string Message { get; private set; }
    }
}
