using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payment : BaseEntity<int>
    {
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public PaymentType PaymentType { get; private set; }
        public int BusinessPartnerId { get; private set; }
        [ForeignKey("BusinessPartnerId")]
        public BusinessPartner BusinessPartner { get; private set; }
        public Guid AccountId { get; private set; }
        [ForeignKey("AccountId")]
        public Level4 Account { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public PaymentRegisterType PaymentRegisterType { get; private set; }
        public Guid PaymentRegisterId { get; private set; }
        [ForeignKey("PaymentRegisterId")]
        public Level4 PaymentRegister { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossPayment { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal IncomeTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetPayment { get; private set; }
        public DocumentStatus Status { get; private set; }

        protected Payment()
        {

        }
        public void setStatus(DocumentStatus status)
        {
            Status = status;
        }

        public void CreateDocNo()
        {
            if (PaymentType == PaymentType.Inflow)
            {

                //Creating doc no..
                DocNo = "PR-" + String.Format("{0:000}", Id);
            }
            else
            {
                DocNo = "PV-" + String.Format("{0:000}", Id);
            }
        }
    }
}
