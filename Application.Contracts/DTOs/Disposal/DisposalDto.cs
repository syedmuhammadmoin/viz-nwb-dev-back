using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class DisposalDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public int AssetId { get; set; }
        public string Asset { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public decimal PurchaseCost { get; set; }
        public int SalvageValue { get; set; }
        public int UsefulLife { get; set; }
        public Guid AccumulatedDepreciationId { get; set; }
        public string AccumulatedDepreciation { get; set; }
        public decimal BookValue { get; set; }
        public DateTime DisposalDate { get; set; }
        public decimal DisposalValue { get; set; }
        public int WarehouseId { get; set; }
        public string Warehouse { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public bool IsAllowedRole { get; set; }
    }   
}
