using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Domain.Interfaces;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BalanceSheetReportService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        //SBBU-Code
        //public Response<List<BalanceSheetDto>> GetBalanceSheet(BalanceSheetFilters filters)
        //{
        //    filters.DocDate = filters.DocDate?.Date;
        //    var campuses = new List<int?>();

        //    if (filters.CampusId != null)
        //    {
        //        campuses.Add(filters.CampusId);
        //    }
        //    //Calling general ledger view
        //    var generalLedger = _unitOfWork.Ledger.Find(new LedgerSpecs())
        //        .Select(i => new GeneralLedgerDto()
        //        {
        //            LedgerId = i.Id,
        //            AccountId = i.Level4_id,
        //            AccountName = i.Level4.Name,
        //            Level1Id = i.Level4.Level1_id,
        //            Nature = i.Level4.Level1.Name,
        //            TransactionId = i.TransactionId,
        //            CampusId = i.CampusId,
        //            WarehouseId = i.WarehouseId,
        //            DocDate = i.TransactionDate,
        //            DocType = i.Transactions.DocType,
        //            DocNo = i.Transactions.DocNo,
        //            Description = i.Description,
        //            Debit = i.Sign == 'D' ? i.Amount : 0,
        //            Credit = i.Sign == 'C' ? i.Amount : 0,
        //            BId = i.BusinessPartnerId,
        //            BusinessPartnerName = i.BusinessPartner != null ? i.BusinessPartner.Name : "N/A",
        //            WarehouseName = i.Warehouse != null ? i.Warehouse.Name : "N/A",
        //            CampusName = i.Campus != null ? i.Campus.Name : "N/A",
        //            Balance = i.Sign == 'D' ? i.Amount : (-1) * i.Amount
        //        });


        //    decimal currentTotal = 0;
        //    //Applying Union
        //    var generalLedgerView = generalLedger
        //        .OrderBy(e => e.LedgerId)
        //        .ThenBy(e => e.DocDate)
        //        .ThenBy(e => e.TransactionId)
        //        .Select(gl =>
        //        {
        //            currentTotal += (gl.Debit - gl.Credit);
        //            gl.Balance = currentTotal;
        //            return gl;
        //        });

        //    var profitNLoss = from glv in generalLedgerView
        //                      where (campuses.Count() > 0 ? campuses.Contains(glv.CampusId) : true) &&
        //                      (glv.DocDate <= filters.DocDate) &&
        //                      (glv.Level1Id == "40000000-5566-7788-99AA-BBCCDDEEFF00") || glv.Level1Id == new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"))
        //                      select new
        //                      {
        //                          glv
        //                      }
        //                      into t1
        //                      group t1 by new
        //                      {
        //                          Nature = t1.glv.Nature,
        //                      } into iGroup
        //                      orderby iGroup.Key.Nature descending
        //                      select new BalanceSheetDto
        //                      {
        //                          Transactional = iGroup.Key.Nature,
        //                          Debit = iGroup.Sum(e => e.glv.Debit),
        //                          Credit = iGroup.Sum(e => e.glv.Credit),
        //                          Balance = iGroup.Key.Nature == "Revenue" ? (iGroup.Sum(e => e.glv.Credit) - iGroup.Sum(e => e.glv.Debit)) : (iGroup.Sum(e => e.glv.Debit) - iGroup.Sum(e => e.glv.Credit))
        //                      };

        //    var revenue = profitNLoss
        //        .Where(e => e.Nature == "Revenue")
        //        .Select(e => e.Balance)
        //        .FirstOrDefault();

        //    var expense = profitNLoss
        //        .Where(e => e.Nature != "Revenue")
        //        .Select(e => e.Balance)
        //        .FirstOrDefault();

        //    var pNl = new BalanceSheetDto()
        //    {
        //        Nature = "Deficit/Surplus",
        //        Transactional = "N/A",
        //        Debit = 0,
        //        Credit = 0,
        //        Balance = revenue - expense
        //    };

        //    var result = (from glv in generalLedgerView
        //                  where (campuses.Count() > 0 ? campuses.Contains(glv.CampusId) : true) &&
        //                  (glv.DocDate <= filters.DocDate) &&
        //                  (glv.Level1Id == new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00") || glv.Level1Id == new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00")
        //                  || glv.Level1Id == new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00"))
        //                  select new
        //                  {
        //                      glv

        //                  } into t1
        //                  group t1 by new
        //                  {
        //                      Nature = t1.glv.Nature,
        //                      Level1Id = t1.glv.Level1Id,
        //                      Transactional = t1.glv.AccountName,
        //                  } into iGroup
        //                  orderby iGroup.Key.Level1Id descending, iGroup.Key.Transactional
        //                  select new BalanceSheetDto
        //                  {
        //                      Nature = iGroup.Key.Nature,
        //                      Transactional = iGroup.Key.Transactional,
        //                      Debit = iGroup.Sum(e => e.glv.Debit),
        //                      Credit = iGroup.Sum(e => e.glv.Credit),
        //                      Balance = iGroup.Key.Nature == "Assets" ? (iGroup.Sum(e => e.glv.Debit) - iGroup.Sum(e => e.glv.Credit)) :
        //                      (iGroup.Sum(e => e.glv.Credit) - iGroup.Sum(e => e.glv.Debit))
        //                  }).ToList();

        //    result.Add(pNl);

        //    return new Response<List<BalanceSheetDto>>(result, "Return Balance Sheet");
        //}
        public Response<List<BalanceSheetDto>> GetBalanceSheet(BalanceSheetFilters balanceSheet)
        {
            balanceSheet.DocDate = balanceSheet.DocDate.Date;

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
                    DepartmentName = i.Location == null ? "N/A" : i.Location.Warehouse.Department.Name,
                    LocationName = i.Location != null ? i.Location.Name : "N/A",
                    WarehouseName = (i.Location != null && i.Location.Warehouse != null) ? i.Location.Warehouse.Name : "N/A",
                    DepartmentId = (i.Location != null && i.Location.Warehouse.Department != null) ? i.Location.Warehouse.DepartmentId : 0,
                    WarehouseId = i.Location != null ? i.Location.WarehouseId : 0,
                    LocationId = i.LocationId,
                    DocDate = i.TransactionDate,
                    DocType = i.Transactions.DocType,
                    DocNo = i.Transactions.DocNo,
                    Description = i.Description,
                    Debit = i.Sign == 'D' ? i.Amount : 0,
                    Credit = i.Sign == 'C' ? i.Amount : 0,
                    BId = i.BusinessPartnerId,
                    BusinessPartnerName = i.BusinessPartner != null ? i.BusinessPartner.Name : "N/A",
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
                              where (glv.DocDate <= balanceSheet.DocDate) &&
                              (glv.Level1Id == "40000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{GetTenant.GetTenantId(_httpContextAccessor)}"
                              || glv.Level1Id == "50000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{GetTenant.GetTenantId(_httpContextAccessor)}")
                              select new
                              {
                                  glv
                              }
                              into t1
                              group t1 by new
                              {
                                  Level1Id = t1.glv.Level1Id,
                              } into iGroup
                              orderby iGroup.Key.Level1Id descending
                              select new BalanceSheetDto
                              {
                                  Nature = iGroup.Key.Level1Id,
                                  Debit = iGroup.Sum(e => e.glv.Debit),
                                  Credit = iGroup.Sum(e => e.glv.Credit),
                                  Balance = iGroup.Key.Level1Id == "40000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{GetTenant.GetTenantId(_httpContextAccessor)}"
                                  ? (iGroup.Sum(e => e.glv.Credit) - iGroup.Sum(e => e.glv.Debit)) : (iGroup.Sum(e => e.glv.Debit) - iGroup.Sum(e => e.glv.Credit))
                              };

            var revenue = profitNLoss
                .Where(e => e.Nature == "40000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{GetTenant.GetTenantId(_httpContextAccessor)}")
                .Select(e => e.Balance)
                .FirstOrDefault();

            var expense = profitNLoss
                .Where(e => e.Nature != "40000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{GetTenant.GetTenantId(_httpContextAccessor)}")
                .Select(e => e.Balance)
                .FirstOrDefault();

            var pNl = new BalanceSheetDto()
            {
                Nature = "Deficit/Surplus",
                Transactional = "N/A",
                Debit = 0,
                Credit = 0,
                Balance = revenue - expense
            };

            var result = (from glv in generalLedgerView
                          where (glv.DocDate <= balanceSheet.DocDate) &&
                          (glv.Level1Id == "10000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{GetTenant.GetTenantId(_httpContextAccessor)}"
                          || glv.Level1Id == "20000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{GetTenant.GetTenantId(_httpContextAccessor)}"
                          || glv.Level1Id == "30000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{GetTenant.GetTenantId(_httpContextAccessor)}")
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
                              Balance = iGroup.Key.Level1Id == "10000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{GetTenant.GetTenantId(_httpContextAccessor)}"
                              ? (iGroup.Sum(e => e.glv.Debit) - iGroup.Sum(e => e.glv.Credit)) :
                              (iGroup.Sum(e => e.glv.Credit) - iGroup.Sum(e => e.glv.Debit))
                          }).ToList();

            result.Add(pNl);

            return new Response<List<BalanceSheetDto>>(result, "Return Balance Sheet");
        }
        Response<List<BalanceSheetSummaryDto>> IBalanceSheetReportService.GetBalanceSheetSummary()
        {

            var balanceSheetSummary =_unitOfWork.Ledger.Find(new LedgerSpecs(FinanceAccountTypes.Assets + $"-{GetTenant.GetTenantId(_httpContextAccessor)}", FinanceAccountTypes.Liability + $"-{GetTenant.GetTenantId(_httpContextAccessor)}", ""))
                                   .GroupBy(i => new 
                                   {
                                       Level1Id = i.Level4.Level1.Id,
                                       Level2Id = i.Level4.Level3.Level2.Id,
                                       Level3Id = i.Level4.Level3.Id,
                                       Level1Name = i.Level4.Level1.Name,
                                       Level2Name = i.Level4.Level3.Level2.Name,
                                       Level3Name = i.Level4.Level3.Name,

                                   })
                                   .Select(group => new BalanceSheetSummaryDto
                                   {
                                       Level1Id = group.Key.Level1Id,
                                       Level2Id = group.Key.Level2Id,
                                       Level3Id = group.Key.Level3Id,
                                       Level1Name = group.Key.Level1Name,
                                       Level2Name = group.Key.Level2Name,
                                       Level3Name = group.Key.Level3Name,
                                       Balance = group.Sum(i => i.Sign == 'D' ? i.Amount : -i.Amount)
                                   })
                                   .ToList();



            return new Response<List<BalanceSheetSummaryDto>>(balanceSheetSummary, "Return Balance Sheet Summary");
        }
    }
}
