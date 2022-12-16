using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateBidEvaluationDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get;  set; }
        [Required]
        [MaxLength(200)]
        public string Title { get;  set; }
        [Required]
        [MaxLength(50)]
        public string RefNo { get;  set; }
        [Required]
        [MaxLength(50)]
        public string MethodOfProcurement { get;  set; }
        [Required]
        [MaxLength(50)]
        public string TendorInquiryNumber { get;  set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "NumberOfBids must be greater than 0")]
        public int? NumberOfBids { get;  set; }
        [Required]
        public DateTime? DateOfOpeningBid { get;  set; }
        [Required]
        public DateTime? DateOfClosingBid { get;  set; }
        [Required]
        [MaxLength(200)]
        public string BidEvaluationCriteria { get;  set; }
        [Required]
        [MaxLength(200)]
        public string LowestEvaluatedBidder { get;  set; }
        [Required]
        public bool? isSubmit { get; set; }
        [Required]
        public virtual List<CreateBidEvaluationLinesDto> BidEvaluationLines { get; set; }
    }
}
