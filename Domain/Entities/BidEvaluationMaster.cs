using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BidEvaluationMaster : BaseEntity<int>
    {
        [MaxLength(50)]
        public string DocNo { get; private set; }
        [MaxLength(200)]
        public string  Name { get; private set; }
        [MaxLength(200)]
        public string Title{ get; private set; }
        [MaxLength(50)]
        public string RefNo { get; private set; }
        [MaxLength(50)]
        public string MethodOfProcurement { get; private set; }
        [MaxLength(50)]
        public string TendorInquiryNumber { get; private set; }
        public int NumberOfBids { get; private set; }
        public DateTime DateOfOpeningBid { get; private set; }
        public DateTime DateOfClosingBid { get; private set; }
        [MaxLength(200)]
        public string BidEvaluationCriteria { get; private set; }
        [MaxLength(200)]
        public string LowestEvaluatedBidder { get; private set; }
        public DocumentStatus State { get; set; }
        public virtual List<BidEvaluationLines> BidEvaluationLines { get; private set; }

        protected BidEvaluationMaster()
        {
        }
        public void SetStatus(DocumentStatus status)
        {
            State = status;
        }
        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "BID-" + String.Format("{0:000}", Id);
        }

    }
}
