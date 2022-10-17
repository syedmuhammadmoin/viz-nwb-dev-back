using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class RemarksSpecs : BaseSpecification<Remark>
    {
        public RemarksSpecs(int docId, DocType docType) 
            : base(x => x.DocId == docId && x.DocType == docType)
        {
            AddInclude(i => i.User);
            ApplyAsNoTracking();
        }
    }
}
