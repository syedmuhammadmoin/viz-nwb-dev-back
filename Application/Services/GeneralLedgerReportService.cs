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
    public class GeneralLedgerReportService : IGeneralLedgerReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private decimal cummulativeBalance = 0;
        private decimal cummulativeDebit = 0;
        private decimal cummulativeCredit = 0;
        public GeneralLedgerReportService(IUnitOfWork unitOfWork)
        {
             _unitOfWork = unitOfWork;
        }
        decimal getCummulativeBalance(decimal e)
        {
            cummulativeBalance = 0;
            return cummulativeBalance += e;
        }
        decimal getCummulativeDebit(decimal e)
        {
            cummulativeDebit = 0;
            return cummulativeDebit += e;
        }
        decimal getCummulativeCredit(decimal e)
        {
            cummulativeCredit = 0;
            return cummulativeCredit += e;
        }
        public Response<List<GeneralLedgerDto>> GetLedger(GeneralLedgerFilters filters)
        {
            //Calling general ledger view
            var generalLedger = _unitOfWork.Ledger.Find(new LedgerSpecs())
                .Select(i => new GeneralLedgerDto()
                {
                    LedgerId = i.Id,
                    AccountId = i.Level4_id,
                    AccountName = i.Level4.Name,
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


            //Getting Data for Opening Balance
            var forOpeningBalance = generalLedgerView
            .Where(e => (e.AccountName.Contains(filters.AccountName)) &&
            (e.BusinessPartnerName.Contains(filters.BusinessPartnerName)) &&
            (e.CampusName.Contains(filters.CampusName)) &&
            (e.WarehouseName.Contains(filters.WarehouseName)) &&
            (e.DocDate < filters.DocDate))
            .OrderBy(x => x.DocDate)
            .ThenBy(x => x.TransactionId)
            .GroupBy(x => x.AccountId)
                .Select(group => new { Group = group, Count = group.Count() })
                .SelectMany(groupWithCount =>
                groupWithCount.Group.Select(b => b)
                .Zip(
                    Enumerable.Range(1, groupWithCount.Count),
                        (j, i) => new GeneralLedgerDto
                        {
                            LedgerId = i,
                            AccountId = j.AccountId,
                            AccountName = j.AccountName,
                            DocType = null,
                            Description = "Initial Balance",
                            Debit = i == 1 ? getCummulativeDebit(j.Debit) : cummulativeDebit += (j.Debit),
                            Credit = i == 1 ? getCummulativeCredit(j.Credit) : cummulativeCredit += (j.Credit),
                            Balance = i == 1 ? getCummulativeBalance(j.Debit - j.Credit) : cummulativeBalance += (j.Debit - j.Credit),
                            IsOpeningBalance = true
                        }
                    ));

            //Getting Opening Balance of every account
            var openingBalance = from element in forOpeningBalance
                                 group element by element.AccountName into groups
                                 select groups.OrderByDescending(p => p.LedgerId).First();

            //Getting data for the given data range
            var glWithOutOpeningBalance = generalLedgerView
                .Where(e => (e.AccountName.Contains(filters.AccountName)) &&
                (e.BusinessPartnerName.Contains(filters.BusinessPartnerName)) &&
                (e.CampusName.Contains(filters.CampusName)) &&
                (e.WarehouseName.Contains(filters.WarehouseName)))
                .OrderBy(x => x.DocDate)
                .ThenBy(x => x.TransactionId)
                .GroupBy(x => x.AccountId)
                .Select(group => new { Group = group, Count = group.Count() })
                .SelectMany(groupWithCount =>
                    groupWithCount.Group.Select(b => b)
                    .Zip(
                        Enumerable.Range(1, groupWithCount.Count),
                        (j, i) => new GeneralLedgerDto
                        {
                            LedgerId = j.LedgerId,
                            AccountId = j.AccountId,
                            AccountName = j.AccountName,
                            TransactionId = j.TransactionId,
                            DocDate = j.DocDate,
                            DocNo = j.DocNo,
                            DocType = j.DocType,
                            Description = j.Description,
                            Debit = j.Debit,
                            Credit = j.Credit,
                            Balance = i == 1 ? getCummulativeBalance(j.Debit - j.Credit) : cummulativeBalance += (j.Debit - j.Credit),
                            BId = j.BId,
                            BusinessPartnerName = j.BusinessPartnerName,
                            CampusName = j.CampusName,
                            WarehouseName = j.WarehouseName,
                        }
                        ));

            var glWithOutOpeningBalance2 = glWithOutOpeningBalance
                .Where(e => (e.AccountName.Contains(filters.AccountName)) &&
                (e.BusinessPartnerName.Contains(filters.BusinessPartnerName)) &&
                (e.CampusName.Contains(filters.CampusName)) &&
                (e.WarehouseName.Contains(filters.WarehouseName)) &&
                (e.DocDate >= filters.DocDate && e.DocDate <= filters.DocDate2));


            //Merging and data with opening balance
            var result = openingBalance
                .Concat(glWithOutOpeningBalance2)
                .OrderBy(e => e.AccountName)
                .ThenBy(e => e.DocDate)
                .ThenBy(e => e.TransactionId)
                .GroupBy(x => x.AccountName)
                .ToList();
            //Declaring List for filter accounts
            List<GeneralLedgerDto> filteredAccount = new List<GeneralLedgerDto>();

            //add filter account in list
            foreach (var item in result)
            {
                Console.WriteLine(item);
                foreach (var i in item)
                {
                    filteredAccount.Add(i);
                }
            }

            return new Response<List<GeneralLedgerDto>>(filteredAccount, "Returning list");
        }
    }
}
