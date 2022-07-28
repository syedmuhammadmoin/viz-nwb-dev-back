using Application.Contracts.DTOs;
using Application.Contracts.DTOs.FileUpload;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FileUploadService : IFileuploadServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _Configuration;

        public FileUploadService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _Configuration = configuration;
        }

        public async Task<Response<FileUploadDto>> DownloadFile(int id)
        {
            var file = await _unitOfWork.Fileupload.GetById(id);
            if (file == null)
                return new Response<FileUploadDto>("No file found");

            var fileDto = _mapper.Map<FileUploadDto>(file);

            return new Response<FileUploadDto>(fileDto, "Returning value");
        }

        public async Task<Response<int>> UploadFile(IFormFile file, int? id, DocType docType)
        {
            if (id == null || id < 0)
                return new Response<int>("Id not found");

            if (file.Length > 5242880)
                return new Response<int>("File size must be less than 5mb");
            _unitOfWork.CreateTransaction();
            try
            {
                string[] supportedTypes = { "txt", "doc", "docx", "pdf", "xls", "xlsx", "png", "jpeg", "jpg" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);

                if (!supportedTypes.Contains(fileExt))
                    return new Response<int>("File format not supported");

                Guid obj = Guid.NewGuid();
                var path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
                string basePath = Path.Combine(path + "\\File\\Error\\");
                string filedir = _Configuration["FilePath"];
                switch (docType)
                {
                    case DocType.Invoice:
                        basePath = Path.Combine(path + filedir + "Invoice\\");
                        break;
                    case DocType.Bill:
                        basePath = Path.Combine(path + filedir + "Bill\\");
                        break;
                    case DocType.CreditNote:
                        basePath = Path.Combine(path + filedir + "CreditNote\\");
                        break;
                    case DocType.DebitNote:
                        basePath = Path.Combine(path + filedir + "DebitNote\\");
                        break;
                    case DocType.Payment:
                        basePath = Path.Combine(path + filedir + "Payment\\");
                        break;
                    case DocType.Receipt:
                        basePath = Path.Combine(path + filedir + "Receipt\\");
                        break;
                    case DocType.PayrollPayment:
                        basePath = Path.Combine(path + filedir + "PayrollPayment\\");
                        break;
                    case DocType.JournalEntry:
                        basePath = Path.Combine(path + filedir + "JournalEntry\\");
                        break;
                    case DocType.PurchaseOrder:
                        basePath = Path.Combine(path + filedir + "PurchaseOrder\\");
                        break;
                    case DocType.GRN:
                        basePath = Path.Combine(path + filedir + "GoodsReceivedNote\\");
                        break;
                    case DocType.GoodsReturnNote:
                        basePath = Path.Combine(path + filedir + "GoodsReturnNote\\");
                        break;
                }

                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = file.FileName + "-" + DateTime.Now.ToString("yyyy-MM-dd");
                var filePath = Path.Combine(basePath, fileName);
                var extension = Path.GetExtension(file.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    // Creating object of getUSer class
                    var getUser = new GetUser(_httpContextAccessor);

                    var userId = getUser.GetCurrentUserId();

                    var fileModel = new FileUpload
                    (
                        (int)id,
                        docType,
                        fileName,
                        file.ContentType,
                        extension,
                        userId
                    );
                    await _unitOfWork.Fileupload.Add(fileModel);
                    await _unitOfWork.SaveAsync();

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    //Commiting the transaction 
                    _unitOfWork.Commit();
                }
                return new Response<int>(1, "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<int>(ex.Message);
            }
        }
    }
}