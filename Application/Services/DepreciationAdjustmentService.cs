using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class DepreciationAdjustmentService : IDepreciationAdjustmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepreciationAdjustmentService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PaginationResponse<List<DepreciationAdjustmentDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var depreciationAdjustment = await _unitOfWork.DepreciationAdjustment.GetAll(new DepreciationAdjustmentSpecs(filter, false));

            if (!depreciationAdjustment.Any())
                return new PaginationResponse<List<DepreciationAdjustmentDto>>(_mapper.Map<List<DepreciationAdjustmentDto>>(depreciationAdjustment), "List is empty");

            var totalRecords = await _unitOfWork.DepreciationAdjustment.TotalRecord(new DepreciationAdjustmentSpecs(filter, true));

            return new PaginationResponse<List<DepreciationAdjustmentDto>>(_mapper.Map<List<DepreciationAdjustmentDto>>(depreciationAdjustment),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DepreciationAdjustmentDto>> GetByIdAsync(int id)
        {
            var specification = new DepreciationAdjustmentSpecs(false);
            var depreciationAdjustment = await _unitOfWork.DepreciationAdjustment.GetById(id, specification);
            if (depreciationAdjustment == null)
                return new Response<DepreciationAdjustmentDto>("Not found");

            var depreciationAdjustmentDto = _mapper.Map<DepreciationAdjustmentDto>(depreciationAdjustment);
            
            ReturningRemarks(depreciationAdjustmentDto);
            depreciationAdjustmentDto.IsAllowedRole = false;
            
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.DepreciationAdjustment)).FirstOrDefault();
            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == depreciationAdjustmentDto.StatusId));
                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            depreciationAdjustmentDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<DepreciationAdjustmentDto>(depreciationAdjustmentDto, "Returning value");
        }

        public async Task<Response<DepreciationAdjustmentDto>> CreateAsync(CreateDepreciationAdjustmentDto entity)
        {
            if ((bool)entity.IsSubmit)
            {
                return await Submit(entity);
            }
            else
            {
                return await Save(entity, 1);
            }
        }

        public async Task<Response<DepreciationAdjustmentDto>> UpdateAsync(CreateDepreciationAdjustmentDto entity)
        {
            if ((bool)entity.IsSubmit)
            {
                return await Submit(entity);
            }
            else
            {
                return await Update(entity, 1);
            }
        }
        
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        
        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getDepreciationAdjustment = await _unitOfWork.DepreciationAdjustment.GetById(data.DocId, new DepreciationAdjustmentSpecs());
            if (getDepreciationAdjustment == null)
            {
                return new Response<bool>("Depreciation Adjustment with the input id not found");
            }
            
            if (getDepreciationAdjustment.Status.State == DocumentStatus.Unpaid 
                || getDepreciationAdjustment.Status.State == DocumentStatus.Partial 
                || getDepreciationAdjustment.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Depreciation Adjustment already approved");
            }

            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.DepreciationAdjustment)).FirstOrDefault();
            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }

            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getDepreciationAdjustment.StatusId 
                    && x.Action == data.Action));
            if (transition == null)
            {
                return new Response<bool>("No transition found");
            }

            // Creating object of getUSer class
            var getUser = new GetUser(_httpContextAccessor);
            var userId = getUser.GetCurrentUserId();
            var currentUserRoles = new GetUser(_httpContextAccessor).GetCurrentUserRoles();
            
            _unitOfWork.CreateTransaction();
            foreach (var role in currentUserRoles)
            {
                if (transition.AllowedRole.Name == role)
                {
                    getDepreciationAdjustment.SetStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getDepreciationAdjustment.Id,
                            DocType = DocType.DepreciationAdjustment,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }
                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        await AddToLedger(getDepreciationAdjustment);
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Depreciation Adjustment Approved");
                    }

                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Depreciation Adjustment Rejected");
                    }

                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();
                    return new Response<bool>(true, "Depreciation Adjustment Reviewed");
                }
            }
            return new Response<bool>("User does not have allowed role");
        }

        //Private methods
        private async Task<Response<DepreciationAdjustmentDto>> Submit(CreateDepreciationAdjustmentDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.DepreciationAdjustment)).FirstOrDefault();
            if (checkingActiveWorkFlows == null)
                return new Response<DepreciationAdjustmentDto>("No workflow found for Depreciation Adjustment");

            if (entity.Id == null)
            {
                return await Save(entity, 6);
            }
            else
            {
                return await Update(entity, 6);
            }
        }
        
        private async Task<Response<DepreciationAdjustmentDto>> Save(CreateDepreciationAdjustmentDto entity, int status)
        {
            //Checking validations
            var validation = await CheckValidations(entity);
            if (!validation.IsSuccess)
                return new Response<DepreciationAdjustmentDto>(validation.Message);

            //Mapping values
            var depreciationAdjustment = _mapper.Map<DepreciationAdjustmentMaster>(entity);
            //Setting status
            depreciationAdjustment.SetStatus(status);

            _unitOfWork.CreateTransaction();

            //Saving in table
            var result = await _unitOfWork.DepreciationAdjustment.Add(depreciationAdjustment);
            await _unitOfWork.SaveAsync();

            //For creating docNo
            depreciationAdjustment.CreateDocNo();
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<DepreciationAdjustmentDto>(_mapper.Map<DepreciationAdjustmentDto>(result), "Created successfully");
        }
        
        private async Task<Response<DepreciationAdjustmentDto>> Update(CreateDepreciationAdjustmentDto entity, int status)
        {
            //Checking validations
            var validation = await CheckValidations(entity);
            if (!validation.IsSuccess)
                return new Response<DepreciationAdjustmentDto>(validation.Message);

            //Get depreciation adjustment
            var specification = new DepreciationAdjustmentSpecs(true);
            var depreciationAdjustment = await _unitOfWork.DepreciationAdjustment.GetById((int)entity.Id, specification);

            if (depreciationAdjustment == null)
                return new Response<DepreciationAdjustmentDto>("Not found");

            if (depreciationAdjustment.StatusId != 1 && depreciationAdjustment.StatusId != 2)
                return new Response<DepreciationAdjustmentDto>("Only draft document can be edited");

            depreciationAdjustment.SetStatus(status);

            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map(entity, depreciationAdjustment);

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<DepreciationAdjustmentDto>(_mapper.Map<DepreciationAdjustmentDto>(depreciationAdjustment), "Created successfully");
        }
        
        private List<RemarksDto> ReturningRemarks(DepreciationAdjustmentDto data)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.DepreciationAdjustment))
                    .Select(e => new RemarksDto()
                    {
                        Remarks = e.Remarks,
                        UserName = e.User.UserName,
                        CreatedAt = e.CreatedDate == null ? "N/A" : ((DateTime)e.CreatedDate).ToString("ddd, dd MMM yyyy")
                    }).ToList();

            if (remarks.Count() > 0)
            {
                data.RemarksList = _mapper.Map<List<RemarksDto>>(remarks);
            }

            return remarks;
        }

        private async Task AddToLedger(DepreciationAdjustmentMaster entity)
        {
            var transaction = new Transactions(entity.Id, entity.DocNo, DocType.DepreciationAdjustment);
            var addTransaction = await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            entity.SetTransactionId(transaction.Id);
            await _unitOfWork.SaveAsync();

            //Inserting data into recordledger table
            List<RecordLedger> recordLedger = entity.DepreciationAdjustmentLines
                .Select(line => new RecordLedger(
                    transaction.Id,
                    line.Level4Id,
                    null,
                    line.FixedAsset.WarehouseId,
                    line.Description,
                    line.Debit > 0 && line.Credit <= 0 ? 'D' : 'C',
                    line.Debit > 0 && line.Credit <= 0 ? line.Debit : line.Credit,
                    line.FixedAsset.Warehouse.CampusId,
                    entity.DateOfDepreciationAdjustment
                    )).ToList();

            await _unitOfWork.Ledger.AddRange(recordLedger);
            await _unitOfWork.SaveAsync();
        }

        private async Task<Response<DepreciationAdjustmentDto>> CheckValidations(CreateDepreciationAdjustmentDto entity)
        {
            if (entity.DepreciationAdjustmentLines.Count() == 0)
                return new Response<DepreciationAdjustmentDto>("Lines are required");

            //Checking duplicate Lines if any
            var duplicates = entity.DepreciationAdjustmentLines.GroupBy(x => new { x.FixedAssetId, x.Level4Id })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();
            if (duplicates.Any())
                return new Response<DepreciationAdjustmentDto>("Duplicate Lines found");

            // Create a dictionary to keep track of the count of each item
            int lineNo = 1;
            Dictionary<int, int> counts = new Dictionary<int, int>();
            foreach (var line in entity.DepreciationAdjustmentLines)
            {
                if (line.Debit == line.Credit)
                    return new Response<DepreciationAdjustmentDto>($"Error on line no {lineNo}: Debit and Credit amount should be in seperate lines");

                var fixedAsset = await _unitOfWork.FixedAsset.GetById((int)line.FixedAssetId);
                if (fixedAsset == null)
                    return new Response<DepreciationAdjustmentDto>($"Error on line no {lineNo}: Invalid fixed asset");

                if (line.Level4Id != fixedAsset.AccumulatedDepreciationId
                    || line.Level4Id != fixedAsset.DepreciationExpenseId)
                {
                    return new Response<DepreciationAdjustmentDto>($"Error on line no {lineNo}: Must select Accumulated Depreciation Account or Depreciation Expense Account");
                }

                if (counts.ContainsKey((int)line.FixedAssetId))
                {
                    counts[(int)line.FixedAssetId]++;
                }
                else
                {
                    counts[(int)line.FixedAssetId] = 1;
                }
                lineNo++;
            }

            // Check if every count is equal to 2
            var isValid = true;
            foreach (int count in counts.Values)
            {
                if (count != 2)
                {
                    isValid = false;
                    break;
                }
            }

            // Print the validation result
            if (!isValid)
                return new Response<DepreciationAdjustmentDto>("Add both account entry for fixed asset");

            if (entity.DepreciationAdjustmentLines.Sum(i => i.Debit) != entity.DepreciationAdjustmentLines.Sum(i => i.Credit))
                return new Response<DepreciationAdjustmentDto>("Sum of debit and credit must be equal");

            return new Response<DepreciationAdjustmentDto>(null, "");
        }

    }
}
