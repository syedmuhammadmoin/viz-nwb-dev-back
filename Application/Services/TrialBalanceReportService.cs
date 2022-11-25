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
    public class TrialBalanceReportService : ITrialBalanceReportService
    {

        private readonly IUnitOfWork _unitOfWork;

        public TrialBalanceReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Response<List<TrialBalanceDto>> GetTrialBalance(TrialBalanceFilters filters)
        {
            filters.DocDate = filters.DocDate?.Date;
            filters.DocDate2 = filters.DocDate2?.Date;
            var accounts = new List<Guid?>();
            var campuses = new List<int?>();
            if (filters.DocDate > filters.DocDate2)
            {
                return new Response<List<TrialBalanceDto>>("Start date is greater than end date");

            }

            if (filters.AccountId != null)
            {
                accounts.Add(filters.AccountId);
            }

            if (filters.CampusId != null)
            {
                campuses.Add(filters.CampusId);
            }

            //Calling general ledger view
            var generalLedger = _unitOfWork.Ledger.Find(new LedgerSpecs())
                .Select(i => new GeneralLedgerDto()
                {
                    LedgerId = i.Id,
                    AccountId = i.Level4_id,
                    AccountName = i.Level4.Name,
                    TransactionId = i.TransactionId,
                    CampusId = i.CampusId,
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


            var forOpeningBalance = generalLedgerView
            .Where(e =>
            (accounts.Count() > 0 ? accounts.Contains(e.AccountId) : true) &&
            (campuses.Count() > 0 ? campuses.Contains(e.CampusId) : true) &&
            (e.DocDate < filters.DocDate))
            .OrderBy(x => x.DocDate)
            .ThenBy(x => x.TransactionId)
            .GroupBy(x => new { x.AccountName, x.AccountId })
            .Select(x => new TrialBalanceDto()
            {
                AccountId = x.Key.AccountId,
                AccountName = x.Key.AccountName,
                DebitOB = x.Sum(e => e.Debit),
                CreditOB = x.Sum(e => e.Credit)
            });

            var trialBalanceWithOutOpeningBalance = generalLedgerView
                .Where(e =>
                (accounts.Count() > 0 ? accounts.Contains(e.AccountId) : true) &&
                (campuses.Count() > 0 ? campuses.Contains(e.CampusId) : true) &&
                (e.DocDate >= filters.DocDate && e.DocDate <= filters.DocDate2))
                .OrderBy(x => x.DocDate)
                .ThenBy(x => x.TransactionId)
                .GroupBy(x => new { x.AccountName, x.AccountId })
                .Select(x => new TrialBalanceDto()
                {
                    AccountId = x.Key.AccountId,
                    AccountName = x.Key.AccountName,
                    Debit = x.Sum(e => e.Debit),
                    Credit = x.Sum(e => e.Credit)
                });
            var groupOpeningBalance = forOpeningBalance
               .Concat(trialBalanceWithOutOpeningBalance)
               .OrderBy(e => e.AccountName)
               .ThenBy(e => e.DocDate)
               .GroupBy(x => new { x.AccountName, x.AccountId })
               .Select(x => new TrialBalanceDto()
               {
                   AccountId = x.Key.AccountId,
                   AccountName = x.Key.AccountName,
                   DebitOB = (x.Sum(e => e.DebitOB) - x.Sum(e => e.CreditOB)) > 0 ? (x.Sum(e => e.DebitOB) - x.Sum(e => e.CreditOB)) : 0,
                   CreditOB = (x.Sum(e => e.DebitOB) - x.Sum(e => e.CreditOB)) > 0 ? 0 : ((x.Sum(e => e.DebitOB) - x.Sum(e => e.CreditOB)) * -1),
                   Debit = x.Sum(e => e.Debit),
                   Credit = x.Sum(e => e.Credit),
                   DebitCB = ((x.Sum(e => e.DebitOB) + x.Sum(e => e.Debit)) - (x.Sum(e => e.CreditOB) + x.Sum(e => e.Credit))) > 0 ?
                   (x.Sum(e => e.DebitOB) + x.Sum(e => e.Debit)) - (x.Sum(e => e.CreditOB) + x.Sum(e => e.Credit)) : 0,
                   CreditCB = ((x.Sum(e => e.DebitOB) + x.Sum(e => e.Debit)) - (x.Sum(e => e.CreditOB) + x.Sum(e => e.Credit))) > 0 ?
                   0 : ((x.Sum(e => e.DebitOB) + x.Sum(e => e.Debit)) - (x.Sum(e => e.CreditOB) + x.Sum(e => e.Credit))) * -1
               });

            return new Response<List<TrialBalanceDto>>(groupOpeningBalance.ToList(), "Returning List");

        }
    }
}
