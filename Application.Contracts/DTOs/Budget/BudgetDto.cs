using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BudgetDto
    {
        public int Id { get; set; }
        public string BudgetName { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public bool IsAllowedRole { get; set; }
        public virtual List<BudgetLinesDto> BudgetLines { get; set; }
    }
}
