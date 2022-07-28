using Application.Contracts.DTOs;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IFileuploadServices
    {
        Task<Response<FileUploadDto>> DownloadFile(int id);
        Task<Response<int>> UploadFile(IFormFile file, int? id, DocType docType);
    }
}
