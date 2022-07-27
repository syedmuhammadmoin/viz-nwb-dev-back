using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public PaymentType PaymentType { get; set; }
        public int BusinessPartnerId { get; set; }
        public string BusinessPartnerName { get; set; }
        public DocType PaymentFormType { get; set; }
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentRegisterType PaymentRegisterType { get; set; }
        public Guid PaymentRegisterId { get; set; }
        public string PaymentRegisterName { get; set; }
        public string Description { get; set; }
        public int? CampusId { get; set; }
        public string CampusName { get; set; }
        public decimal GrossPayment { get; set; }
        public decimal SalesTax { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal SRBTax { get; set; }
        public decimal NetPayment { get; set; }
        public decimal ReconciledAmount { get; set; }
        public IEnumerable<PaidDocListDto> PaidAmountList { get; set; }
        public decimal UnreconciledAmount { get; set; }
        public PaidDocListDto DocumentReconcile { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public int? TransactionId { get; set; }
        public int? LedgerId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public bool IsAllowedRole { get; set; }

    }
}
