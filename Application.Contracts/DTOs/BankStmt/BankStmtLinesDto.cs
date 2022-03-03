using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BankStmtLinesDto
    {
        public int Id { get; set; }
        public int Reference { get; set; }
        public DateTime StmtDate { get; set; }
        public string Label { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int MasterId { get; set; }

    }
}
