using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class ProgramChallanTemplateRepository : GenericRepository<ProgramChallanTemplateMaster, int>, IProgramChallanTemplateRepository
    {
        public ProgramChallanTemplateRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
