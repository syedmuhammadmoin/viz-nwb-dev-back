using Domain.Entities;
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
    public class DepreciationAdjustmentDto
    {
        public int Id { get; set; }
        public DateTime DateOfDepreciationAdjustment { get;  set; }
        public int CampusId { get;  set; }
        public string Campus { get;  set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public bool IsAllowedRole { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public virtual List<DepreciationAdjustmentLinesDto> DepreciationAdjustmentLines { get;  set; }
    }
}
