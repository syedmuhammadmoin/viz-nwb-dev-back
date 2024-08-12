using Application.Contracts.DTOs;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Specifications;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class COAService : ICOAService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public COAService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<List<Level1Dto>>> GetCOA()
        {
            var coa = await _unitOfWork.Level4.GetCOA();
            return new Response<List<Level1Dto>>(_mapper.Map<List<Level1Dto>>(coa), "Returning chart of account");
        }
        public async Task<MemoryStream> Export() 
        {
            var chartOfAccounts = _unitOfWork.Level4.Find(new Level4Specs(true, 0, NeedtoBeFixed: 0))
                   .Select(i => new ChartOfAccountDto()
                   {
                       Nature = i.Level1.Name,
                       Head = i.Level3.Level2.Name,
                       SummaryHead = i.Level3.Name,
                       TransactionalAccount = i.Name
                   })
                   .ToList();

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(chartOfAccounts, PrintHeaders: true);
                package.Save();
            }
            stream.Position = 0;
            return stream;
        }
    }
}
