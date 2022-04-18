using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Interfaces;
using Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BalanceSheetReportService : IBalanceSheetReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BalanceSheetReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Response<List<BalanceSheetDto>> GetBalanceSheet(BalanceSheetFilters balanceSheet)
        {
            balanceSheet.DocDate = balanceSheet.DocDate.Date;

            if (balanceSheet.CampusName == null)
            {
                balanceSheet.CampusName = "";
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
                    TransactionId = i.Id,
                    CampusId = i.Id,
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

            var profitNLoss = from glv in generalLedgerView
                              where glv.CampusName.Contains(balanceSheet.CampusName) &&
                              (glv.DocDate <= balanceSheet.DocDate) &&
                              (glv.Level1Id == new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00") || glv.Level1Id == new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"))
                              select new
                              {
                                  glv
                              }
                              into t1
                              group t1 by new
                              {
                                  Nature = t1.glv.Nature,
                              } into iGroup
                              orderby iGroup.Key.Nature descending
                              select new BalanceSheetDto
                              {
                                  Transactional = iGroup.Key.Nature,
                                  Debit = iGroup.Sum(e => e.glv.Debit),
                                  Credit = iGroup.Sum(e => e.glv.Credit),
                                  Balance = iGroup.Key.Nature == "REVENUE" ? (iGroup.Sum(e => e.glv.Credit) - iGroup.Sum(e => e.glv.Debit)) : (iGroup.Sum(e => e.glv.Debit) - iGroup.Sum(e => e.glv.Credit))
                              };

            var revenue = profitNLoss
                .Where(e => e.Nature == "REVENUE")
                .Select(e => e.Balance)
                .FirstOrDefault();

            var expense = profitNLoss
                .Where(e => e.Nature != "REVENUE")
                .Select(e => e.Balance)
                .FirstOrDefault();

            var pNl = new BalanceSheetDto()
            {
                Nature = "DEFICIT/SURPLUS",
                Transactional = "N/A",
                Debit = 0,
                Credit = 0,
                Balance = revenue - expense
            };

            var result = (from glv in generalLedgerView
                          where glv.CampusName.Contains(balanceSheet.CampusName) &&
                          (glv.DocDate <= balanceSheet.DocDate) &&
                          (glv.Level1Id == new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00") || glv.Level1Id == new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00")
                          || glv.Level1Id == new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00"))
                          select new
                          {
                              glv
                              
                          } into t1
                          group t1 by new
                          {
                              Nature = t1.glv.Nature,
                              Level1Id = t1.glv.Level1Id,
                              Transactional = t1.glv.AccountName,
                          } into iGroup
                          orderby iGroup.Key.Level1Id descending, iGroup.Key.Transactional
                          select new BalanceSheetDto
                          {
                              Nature = iGroup.Key.Nature,
                              Transactional = iGroup.Key.Transactional,
                              Debit = iGroup.Sum(e => e.glv.Debit),
                              Credit = iGroup.Sum(e => e.glv.Credit),
                              Balance = iGroup.Key.Nature == "ASSETS" ? (iGroup.Sum(e => e.glv.Debit) - iGroup.Sum(e => e.glv.Credit)) :
                              (iGroup.Sum(e => e.glv.Credit) - iGroup.Sum(e => e.glv.Debit))
                          }).ToList();

            result.Add(pNl);

            return new Response<List<BalanceSheetDto>>(result, "Return Balance Sheet");

        }

    }
}
