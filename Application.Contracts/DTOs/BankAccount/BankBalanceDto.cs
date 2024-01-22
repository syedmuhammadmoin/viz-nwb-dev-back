using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.BankAccount
{
    public class BankBalanceDto
    {
        public DateTime Date { get; set; }
        public int BankId { get; set; }    
        public string BankName { get; set; }    
        public bool IsReconcile { get; set; }    
        public decimal Balance { get; set; }    
    }
}
