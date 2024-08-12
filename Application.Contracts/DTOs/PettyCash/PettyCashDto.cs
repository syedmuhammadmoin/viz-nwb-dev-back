using Application.Contracts.DTOs;
using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PettyCashDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public decimal OpeningBalance { get;  set; }
        public decimal ClosingBalance { get;  set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public int TransactionId { get; set; }
        public virtual List<PettyCashLinesDto> PettyCashLines { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public IEnumerable<FileUploadDto> FileUploadList { get; set; }

        public bool IsAllowedRole { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string LastUser
        {

            get { return RemarksList?.LastOrDefault().UserName ?? ModifiedBy ?? CreatedBy; }
        }
    }
}
