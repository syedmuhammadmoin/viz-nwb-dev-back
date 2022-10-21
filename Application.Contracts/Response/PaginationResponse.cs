using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Response
{
    public class PaginationResponse<T> : Response<T>
    {
        public int PageStart { get; private set; }
        public int PageEnd { get; private set; }
        public int TotalRecords { get; private set; }

        public PaginationResponse(T data, int pageStart, int pageEnd, int totalRecords, string message)
        {
            this.PageStart = pageStart;
            this.PageEnd = pageEnd;
            this.TotalRecords = totalRecords;
            this.Result = data;
            this.Message = message;
            this.IsSuccess = true;
            StatusCode = 200;
        }
        public PaginationResponse(T data, string message)
        {
            this.Result = data;
            this.Message = message;
            this.IsSuccess = true;
            StatusCode = 200;
        }

        public PaginationResponse(string message)
        {
            this.Message = message;
            this.IsSuccess = false;
        }
    }
}
