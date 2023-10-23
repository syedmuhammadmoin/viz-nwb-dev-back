using Application.Contracts.DTOs;
using Application.Contracts.Filters;

namespace Application.Contracts.Interfaces
{
    public interface IProgramChallanTemplateService : ICrudService<CreateProgramChallanTemplateDto, ProgramChallanTemplateDto, int, TransactionFormFilter>
    {
    }
}
