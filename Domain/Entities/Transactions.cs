using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Transactions : BaseEntity<int>
    {
        public int DocId { get; set; }
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public DocType DocType { get; private set; }

        protected Transactions()
        {

        }

        public Transactions(int docId, string docNo, DocType docType)
        {
            DocId = docId;
            DocNo = docNo;
            DocType = docType;
        }

        public void UpdateDocNo(int docId, string docNo)
        {
            DocId = docId;
            DocNo = docNo;
        }
    }
    
}
