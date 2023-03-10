using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class BudgetReappropriationDto
    {
        public int Id{ get; set; }
        public int BudgetId { get; set; }
        public string Budget { get; set; }
        public DateTime BudgetReappropriationDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public virtual List<BudgetReappropriationLinesDto> BudgetReappropriationLines { get; set; }
        public bool IsAllowedRole { get; set; }

    }
}
