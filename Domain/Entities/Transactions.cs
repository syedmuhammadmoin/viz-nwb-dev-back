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
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public DocType DocType { get; private set; }

        protected Transactions()
        {

        }

        public Transactions(string docNo, DocType docType)
        {
            DocNo = docNo;
            DocType = docType;
        }
    }
    
}
