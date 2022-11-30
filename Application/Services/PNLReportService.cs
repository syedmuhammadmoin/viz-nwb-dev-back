using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PNLReportService : IPNLReportService
    {

        private readonly IUnitOfWork _unitOfWork;

        public PNLReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Response<List<PNLDto>> GetProfitLoss(PNLFilters filters)
        {
            var accounts = new List<Guid?>();
            var warehouses = new List<int?>();
            var campuses = new List<int?>();
            var businessPartners = new List<int?>();

            if (filters.AccountId != null)
            {
                accounts.Add(filters.AccountId);
            }

            if (filters.WarehouseId != null)
            {
                warehouses.Add(filters.WarehouseId);
            }

            if (filters.CampusId != null)
            {
                campuses.Add(filters.CampusId);
            }

            if (filters.BusinessPartnerId != null)
            {
                businessPartners.Add(filters.BusinessPartnerId);
            }
            //Calling general ledger view
            var generalLedger = _unitOfWork.Ledger.Find(new LedgerSpecs())
                .Select(i => new GeneralLedgerDto()
                {
                    LedgerId = i.Id,
                    AccountId = i.Level4_id,
                    AccountName = i.Level4.Name,
                    Level1Id = i.Level4.Level1_id,
                    Nature = i.Level4.Level1.Name,
                    TransactionId = i.TransactionId,
                    CampusId = i.CampusId,
                    WarehouseId = i.WarehouseId,
                    DocDate = i.TransactionDate,
                    DocType = i.Transactions.DocType,
                    DocNo = i.Transactions.DocNo,
                    Description = i.Description,
                    Debit = i.Sign == 'D' ? i.Amount : 0,
                    Credit = i.Sign == 'C' ? i.Amount : 0,
                    BId = i.BusinessPartnerId,
                    BusinessPartnerName = i.BusinessPartner != null ? i.BusinessPartner.Name : "N/A",
                    WarehouseName = i.Warehouse != null ? i.Warehouse.Name : "N/A",
                    CampusName = i.Campus != null ? i.Campus.Name : "N/A",
                    Balance = i.Sign == 'D' ? i.Amount : (-1) * i.Amount
                });

            decimal currentTotal = 0;
            //Applying Union
            var generalLedgerView = generalLedger
                .OrderBy(e => e.LedgerId)
                .ThenBy(e => e.DocDate)
                .ThenBy(e => e.TransactionId)
                .Select(gl =>
                {
                    currentTotal += (gl.Debit - gl.Credit);
                    gl.Balance = currentTotal;
                    return gl;
                });

            var result = from glv in generalLedgerView
                         where ((accounts.Count() > 0 ? accounts.Contains(glv.AccountId) : true) &&
                        (warehouses.Count() > 0 ? warehouses.Contains(glv.WarehouseId) : true) &&
                        (businessPartners.Count() > 0 ? businessPartners.Contains(glv.BId) : true) &&
                        (campuses.Count() > 0 ? campuses.Contains(glv.CampusId) : true) &&
                         (glv.Level1Id == new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00") || glv.Level1Id == new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")) &&
                         (glv.DocDate >= filters.DocDate && glv.DocDate <= filters.DocDate2))
                         select new
                         {
                             glv,

                         } into t1

                         group t1 by new
                         {
                             Level1Id = t1.glv.Level1Id,
                             Nature = t1.glv.Nature,
                             Transactional = t1.glv.AccountName,
                         } into iGroup
                         orderby iGroup.Key.Transactional
                         select new PNLDto
                         {
                             Nature = iGroup.Key.Nature,
                             BusinessPartnerName = iGroup.Select(x => x.glv.BusinessPartnerName).FirstOrDefault(),
                             WarehouseName = iGroup.Select(x => x.glv.WarehouseName).FirstOrDefault(),
                             CampusName = iGroup.Select(x => x.glv.CampusName).FirstOrDefault(),
                             Transactional = iGroup.Key.Transactional,
                             Debit = iGroup.Sum(e => e.glv.Debit),
                             Credit = iGroup.Sum(e => e.glv.Credit),
                             Balance = iGroup.Key.Level1Id == new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00") ? (iGroup.Sum(e => e.glv.Credit) - iGroup.Sum(e => e.glv.Debit)) : (iGroup.Sum(e => e.glv.Debit) - iGroup.Sum(e => e.glv.Credit))
                         };
            return new Response<List<PNLDto>>(result.ToList(), "Return Profit and Loss");
        }
    }
}
