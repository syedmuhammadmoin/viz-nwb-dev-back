using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBatchRepository : IGenericRepository<BatchMaster, int>
    {
        Task AddCriteriaInBatch(List<BatchAdmissionCriteria> entity);
        Task RemoveCriteriaFromBatch(int batchId);
    }
}
