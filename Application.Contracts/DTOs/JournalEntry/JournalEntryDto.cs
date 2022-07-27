using Application.Contracts.DTOs.FileUpload;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class JournalEntryDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public int TransactionId { get; set; }
        public virtual List<JournalEntryLinesDto> JournalEntryLines { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public IEnumerable<FileUploadDto> FileUploadList { get; set; }

        public bool IsAllowedRole { get; set; }
    }
}
