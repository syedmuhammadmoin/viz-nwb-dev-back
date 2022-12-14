using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateBidEvaluationDtos
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; private set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; private set; }
        [Required]
        [MaxLength(50)]
        public string RefNo { get; private set; }
        [Required]
        [MaxLength(50)]
        public string MethodOfProcurement { get; private set; }
        [Required]
        [MaxLength(50)]
        public string TendorInquiryNumber { get; private set; }
        [Required]
        public int NumberOfBids { get; private set; }
        [Required]
        public DateTime DateOfOpeningBid { get; private set; }
        [Required]
        public DateTime DateOfClosingBid { get; private set; }
        [Required]
        [MaxLength(200)]
        public string BidEvaluationCriteria { get; private set; }
        [Required]
        [MaxLength(200)]
        public string LowestEvaluatedBidder { get; private set; }
        [Required]
        public bool isSubmit { get; set; }
        [Required]
        public virtual List<CreateBidEvaluationLinesDtos> BidEvaluationLines { get; set; }
    }
}
