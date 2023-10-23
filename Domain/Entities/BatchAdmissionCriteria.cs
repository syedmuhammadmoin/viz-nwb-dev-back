using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class BatchAdmissionCriteria : BaseEntity<int>
    {
        public int BatchId { get; private set; }
        [ForeignKey("BatchId")]
        public BatchMaster Batch { get; private set; }
        public int CriteriaId { get; private set; }
        [ForeignKey("CriteriaId")]
        public AdmissionCriteria Criteria { get; private set; }
        
        protected BatchAdmissionCriteria()
        {
        }

        public BatchAdmissionCriteria(int batchId, int criteriaId)
        {
            BatchId = batchId;
            CriteriaId = criteriaId;
        }

    }
}
