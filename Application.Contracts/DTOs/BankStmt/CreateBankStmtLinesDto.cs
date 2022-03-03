using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateBankStmtLinesDto
    {
        public int? Id { get; set; }
        [Required]
        public int Reference { get; set; }
        [Required]
        public DateTime StmtDate { get; set; }
        [Required]
        [MaxLength(50)]
        public string Label { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Credit { get; set; }
    }
}
