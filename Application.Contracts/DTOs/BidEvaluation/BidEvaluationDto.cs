using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BidEvaluationDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string RefNo { get; set; }
        public string MethodOfProcurement { get; set; }
        public string TendorInquiryNumber { get; set; }
        public int NumberOfBids { get; set; }
        public DateTime DateOfOpeningBid { get; set; }
        public DateTime DateOfClosingBid { get; set; }
        public string BidEvaluationCriteria { get; set; }
        public string LowestEvaluatedBidder { get; set; }
        public DocumentStatus State { get; set; }
        public string Status { get; set; }
        public virtual List<BidEvaluationLinesDto> BidEvaluationLines { get; set; }
    }
}