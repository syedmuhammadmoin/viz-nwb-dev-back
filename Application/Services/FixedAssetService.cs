using Application.Contracts.DTOs;
using Application.Contracts.DTOs.FixedAsset;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Application.Services
{
    public class FixedAssetService : IFixedAssetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FixedAssetService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<FixedAssetDto>> CreateAsync(CreateFixedAssetDto entity)
        {
            //Checking workflow
            int status = 1;
            if ((bool)entity.IsSubmit)
            {
                var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.FixedAsset)).FirstOrDefault();
                if (checkingActiveWorkFlows == null)
                {
                    return new Response<FixedAssetDto>("No workflow found for Fixed Asset");
                }
                status = 6;
            }

            //Checking validation
            if (entity.SalvageValue > entity.Cost)
                return new Response<FixedAssetDto>("Salvage value cannot be greater than cost");

            if (entity.DepreciationApplicability)
            {
                if ((entity.DepreciationModelId == null && entity.DepreciationModelId == 0)
                    || (entity.UseFullLife == null && entity.UseFullLife == 0)
                    || entity.DepreciationExpenseId == null)
                {
                    return new Response<FixedAssetDto>("Depreciation Model Fields are Required");
                }
                if (entity.ModelType == DepreciationMethod.Declining && (entity.DecLiningRate == null || entity.DecLiningRate == 0))
                {
                    return new Response<FixedAssetDto>("Declining Rate is Required");
                }
            }
            else
            {
                entity.DepreciationModelId = null;
                entity.AssetAccountId = null;
                entity.DepreciationExpenseId = null;
                entity.AccumulatedDepreciationId = null;
                entity.UseFullLife = null;
                entity.DecLiningRate = 0;
                entity.ModelType = 0;
            }

            _unitOfWork.CreateTransaction();
            if (entity.Quantity == 0)
            {
                return new Response<FixedAssetDto>("Quantity is Required");
            }
            for (int i = 0; i < entity.Quantity; i++)
            {
                var fix = _mapper.Map<FixedAsset>(entity);
                //Setting status
                fix.SetStatus(status);
                await _unitOfWork.FixedAsset.Add(fix);
                await _unitOfWork.SaveAsync();
                //For creating docNo
                fix.CreateCode();
                await _unitOfWork.SaveAsync();
            }

            if (entity.DocId != null)
            {

                if (entity.DocId != null && entity.Doctype == DocType.GRN)
                {
                    var GRNLines = _unitOfWork.GRN
                        .FindLines(new GRNLinesSpecs((int)entity.DocId, (int)entity.ProductId, (int)entity.WarehouseId, false))
                        .FirstOrDefault();

                    if (GRNLines != null)
                    {
                        GRNLines.SetIsFixedAssetCreatedTrue();
                        await _unitOfWork.SaveAsync();
                    }
                }
            }

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<FixedAssetDto>(null, "Created successfully");
        }

        public async Task<Response<FixedAssetDto>> UpdateAsync(UpdateFixedAssetDto entity)
        {
            //Checking workflow
            int status = 1;
            if ((bool)entity.IsSubmit)
            {
                var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.FixedAsset)).FirstOrDefault();
                if (checkingActiveWorkFlows == null)
                {
                    return new Response<FixedAssetDto>("No workflow found for Fixed Asset");
                }
                status = 6;
            }

            //Checking validation
            if (entity.SalvageValue > entity.Cost)
                return new Response<FixedAssetDto>("Salvage value cannot be greater than cost");
            if (entity.DepreciationApplicability)
            {
                if ((entity.DepreciationModelId == null && entity.DepreciationModelId == 0)
                    || (entity.UseFullLife == null && entity.UseFullLife == 0)
                    || entity.DepreciationExpenseId == null)
                {
                    return new Response<FixedAssetDto>("Depreciation Model Fields are Required");
                }
                if (entity.ModelType == DepreciationMethod.Declining && (entity.DecLiningRate == null || entity.DecLiningRate == 0))
                {
                    return new Response<FixedAssetDto>("Declining Rate is Required");
                }
            }
            else
            {
                entity.DepreciationModelId = null;
                entity.AssetAccountId = null;
                entity.DepreciationExpenseId = null;
                entity.AccumulatedDepreciationId = null;
                entity.UseFullLife = null;
                entity.DecLiningRate = 0;
                entity.ModelType = 0;
            }

            //Getting fixed asset
            var result = await _unitOfWork.FixedAsset.GetById((int)entity.Id);
            if (result == null)
                return new Response<FixedAssetDto>("Not found");

            if (result.StatusId != 1 && result.StatusId != 2)
                return new Response<FixedAssetDto>("Only draft document can be edited");

            //Setting status
            result.SetStatus(status);

            _unitOfWork.CreateTransaction();
            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<FixedAssetDto>(_mapper.Map<FixedAssetDto>(result), "Updated successfully");
        }

        public async Task<Response<FixedAssetDto>> UpdateAfterApproval(UpdateSalvageValueDto entity)
        {
            //Getting fixed asset
            var result = await _unitOfWork.FixedAsset.GetById((int)entity.Id, new FixedAssetSpecs());
            if (result == null)
                return new Response<FixedAssetDto>("Not found");

            if (entity.SalvageValue > result.Cost)
                return new Response<FixedAssetDto>("Salvage value cannot be greater than cost");

            if (result.Status.State != DocumentStatus.Unpaid)
                return new Response<FixedAssetDto>("Only approved asset can be edited");
            
            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();

            return new Response<FixedAssetDto>(_mapper.Map<FixedAssetDto>(result), "Updated successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<FixedAssetDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetAll(new FixedAssetSpecs(filter, false));

            if (fixedAsset.Count() == 0)
                return new PaginationResponse<List<FixedAssetDto>>(_mapper.Map<List<FixedAssetDto>>(fixedAsset), "List is empty");

            var totalRecords = await _unitOfWork.FixedAsset.TotalRecord(new FixedAssetSpecs(filter, true));

            return new PaginationResponse<List<FixedAssetDto>>(_mapper.Map<List<FixedAssetDto>>(fixedAsset),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<FixedAssetDto>> GetByIdAsync(int id)
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetById(id, new FixedAssetSpecs());
            if (fixedAsset == null)
                return new Response<FixedAssetDto>("Not found");
            var d = new DateTime();

            //Getting fixed asset lines
            var fixedAssetLines = await _unitOfWork.FixedAssetLines.GetByMonthAndYear(id, d.Month, d.Year);
            var fixedAssetLinesDto = _mapper.Map<List<FixedAssetLinesDto>>(fixedAssetLines);

            //Mappiing fixed asset
            var fixedAssetDto = _mapper.Map<FixedAssetDto>(fixedAsset);

            //Mapping Lines
            fixedAssetDto.FixedAssetlines = fixedAssetLinesDto;

            //Getting Depreciation register
            var depreciationRegister = await _unitOfWork.DepreciationRegister.GetByMonthAndYear(id, d.Month, d.Year);


            //Mappiing fixed asset
            var DepreciationRegisterDto = _mapper.Map<List<DepreciationRegisterDto>>(depreciationRegister);

            //Mapping Lines
            fixedAssetDto.DepriecaitonRegisterList = DepreciationRegisterDto;

            ReturningRemarks(fixedAssetDto);


            if (fixedAssetDto.DepreciationApplicability == false)
            {
                fixedAssetDto.DepreciationModelId = null;
                fixedAssetDto.AssetAccountId = null;
                fixedAssetDto.AccumulatedDepreciation = null;
                fixedAssetDto.DepreciationExpenseId = null;
            }

            fixedAssetDto.IsAllowedRole = false;

            var workflow = _unitOfWork.WorkFlow
                .Find(new WorkFlowSpecs(DocType.FixedAsset))
                .FirstOrDefault();

            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == fixedAssetDto.StatusId));
                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            fixedAssetDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<FixedAssetDto>(fixedAssetDto, "Returning value");
        }

        public async Task<Response<List<FixedAssetDto>>> GetDropDown()
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetAll();
            if (!fixedAsset.Any())
                return new Response<List<FixedAssetDto>>(null, "List is empty");

            return new Response<List<FixedAssetDto>>(_mapper.Map<List<FixedAssetDto>>(fixedAsset), "Returning List");

        }

        public async Task<Response<List<FixedAssetDto>>> GetDisposableAssetDropDown()
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetAll(new FixedAssetSpecs(true));
            if (!fixedAsset.Any())
                return new Response<List<FixedAssetDto>>(null, "List is empty");

            return new Response<List<FixedAssetDto>>(_mapper.Map<List<FixedAssetDto>>(fixedAsset), "Returning List");

        }

        public async Task<Response<List<FixedAssetDto>>> GetAssetByProductIdDropDown(int ProductId)
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetAll(new FixedAssetSpecs(ProductId));
            if (!fixedAsset.Any())
                return new Response<List<FixedAssetDto>>(null, "List is empty");

            return new Response<List<FixedAssetDto>>(_mapper.Map<List<FixedAssetDto>>(fixedAsset), "Returning List");

        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getFixedAsset = await _unitOfWork.FixedAsset.GetById(data.DocId, new FixedAssetSpecs());

            if (getFixedAsset == null)
            {
                return new Response<bool>("Fixed Asset with the input id not found");
            }

            if (getFixedAsset.Status.State == DocumentStatus.Unpaid || getFixedAsset.Status.State == DocumentStatus.Partial || getFixedAsset.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Fixed Asset already approved");
            }

            var workflow = _unitOfWork.WorkFlow
                .Find(new WorkFlowSpecs(DocType.FixedAsset))
                .FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }

            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getFixedAsset.StatusId && x.Action == data.Action));

            if (transition == null)
            {
                return new Response<bool>("No transition found");
            }

            // Creating object of getUSer class
            var getUser = new GetUser(this._httpContextAccessor);

            var userId = getUser.GetCurrentUserId();
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();

            _unitOfWork.CreateTransaction();
            foreach (var role in currentUserRoles)
            {
                if (transition.AllowedRole.Name == role)
                {
                    getFixedAsset.SetStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getFixedAsset.Id,
                            DocType = DocType.FixedAsset,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }

                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Document Approved");
                    }
                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Document Rejected");
                    }
                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();
                    return new Response<bool>(true, "Document Reviewed");
                }
            }

            return new Response<bool>("User does not have allowed role");


        }

        public async Task<Response<bool>> HeldAssetForDisposal(int Id)
        {
            //Getting fixed asset
            var result = await _unitOfWork.FixedAsset.GetById(Id);
            if (result == null)
                return new Response<bool>("Not found");

            if (result.IsHeldforSaleOrDisposal)
                return new Response<bool>("Already held for disposal or sale");

            //Setting status
            result.SetHeldForDisposalTrue();
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true, "Held for disposal successfully");
        }
        
        public async Task<Response<FixedAssetLinesDto>> CreateFixedAssetLinesAsync(CreateFixedAssetLinesDto entity)
        {
            _unitOfWork.CreateTransaction();
            var fixedAssetLines = _mapper.Map<FixedAssetLines>(entity);
            await _unitOfWork.FixedAssetLines.Add(fixedAssetLines);
            await _unitOfWork.SaveAsync();
            //returning response
            return new Response<FixedAssetLinesDto>(null, "Created successfully");

        }
        
        public async Task<Response<DepreciationRegisterDto>> CreateDepreciationRegisterAsync(CreateDepreciationRegisterDto entity)
        {
            _unitOfWork.CreateTransaction();
            var depreciationRegister = _mapper.Map<DepreciationRegister>(entity);
            await _unitOfWork.DepreciationRegister.Add(depreciationRegister);
            await _unitOfWork.SaveAsync();
            //returning response
            return new Response<DepreciationRegisterDto>(null, "Created successfully");

        }
        
        public async Task<Response<FixedAssetDto>> Depreciate(CreateDepreciationRegisterDto createDepreciationRegisterDto)
        {

            var fixedAsset = await _unitOfWork.FixedAsset.GetById(createDepreciationRegisterDto.FixedAssetId, new FixedAssetSpecs());

            var fixedAssetDto = _mapper.Map<FixedAssetDto>(fixedAsset);

            fixedAssetDto.IsGoingtoDisposeAsset = createDepreciationRegisterDto.IsGoingtoDispose;


            if (createDepreciationRegisterDto.IsAutomatedCalculation)
            {
                if (fixedAssetDto.IsDepreciable)
                {
                    //1a. entry in Fixed Lines 
                    var maxActiveRecord = fixedAssetDto.FixedAssetlines.OrderByDescending(r => r.ActiveDate).FirstOrDefault();
                    maxActiveRecord.InactiveDate = fixedAssetDto.CurrentDate;
                    var createFixedAssetlineDto = _mapper.Map<CreateFixedAssetLinesDto>(maxActiveRecord);
                    await CreateFixedAssetLinesAsync(createFixedAssetlineDto);

                    // 1b. add new month active date
                    TimeSpan timeSpan = maxActiveRecord.InactiveDate.Value - maxActiveRecord.ActiveDate;
                    maxActiveRecord.ActiveDays = timeSpan.Days + 1; // add 1 to include both start and end dates
                    createFixedAssetlineDto = new CreateFixedAssetLinesDto() { ActiveDate = fixedAssetDto.CurrentDate.AddDays(1), FixedAssetId = fixedAssetDto.Id };
                    await CreateFixedAssetLinesAsync(createFixedAssetlineDto);

                    // 2.entry in Depreciation Register						
                    createDepreciationRegisterDto.Description = "Depreciation of month" + fixedAssetDto.CurrentDate.Month.ToString() + " / " + fixedAssetDto.CurrentDate.Year.ToString();
                    await CreateDepreciationRegisterAsync(createDepreciationRegisterDto);


                    // 3.entry in the ledger
                    List<RecordLedger> recordLedgers = new List<RecordLedger>() {
                    new RecordLedger(0, fixedAssetDto.AccumulatedDepreciationId.Value, null, null, "Asset" + fixedAssetDto.Id, 'C', fixedAssetDto.DepreciableAmount, fixedAssetDto.WarehouseId, fixedAssetDto.CurrentDate),
                    new RecordLedger(0, fixedAssetDto.DepreciationExpenseId.Value, null, null, "Asset" + fixedAssetDto.Id, 'D', fixedAssetDto.DepreciableAmount, fixedAssetDto.WarehouseId, fixedAssetDto.CurrentDate)

                    };

                    await AddToLedger(recordLedgers);

                    // 4.update accumulated_Depreciation in Fixed Asset	
                    var result = await _unitOfWork.FixedAsset.GetById((int)createDepreciationRegisterDto.FixedAssetId);
                    if (result == null)
                        return new Response<FixedAssetDto>("Not found");
                    result.SetAccumulatedDepreciationAmount(fixedAssetDto.AccumulatedDepreciationAmount + fixedAssetDto.DepreciableAmount);
                    //5.Update Active Days in FixedAsset 
                    result.SetTotalActiveDays(fixedAssetDto.TotalActiveDays + fixedAssetDto.ActiveDaysofMonth());




                    await _unitOfWork.SaveAsync();

                    //Commiting the transaction
                    _unitOfWork.Commit();

                }
                else
                {
                    //Create log // due to working in background 
                }

            }
            else
            {
                // 1.entry in Depreciation Register
                createDepreciationRegisterDto.Description = "Depreciation of month" + fixedAssetDto.CurrentDate.Month.ToString() + " / " + fixedAssetDto.CurrentDate.Year.ToString();
                await CreateDepreciationRegisterAsync(createDepreciationRegisterDto);
                //2.entry in the ledger 							
                List<RecordLedger> recordLedgers = new List<RecordLedger>() {
                    new RecordLedger(0, fixedAssetDto.AccumulatedDepreciationId.Value, null, null, "Asset" + fixedAssetDto.Id, 'C', fixedAssetDto.DepreciableAmount, fixedAssetDto.WarehouseId, fixedAssetDto.CurrentDate),
                    new RecordLedger(0, fixedAssetDto.DepreciationExpenseId.Value, null, null, "Asset" + fixedAssetDto.Id, 'D', fixedAssetDto.DepreciableAmount, fixedAssetDto.WarehouseId, fixedAssetDto.CurrentDate)

                    };
                //this is not compatible for adjustment
                await AddToLedger(recordLedgers);

                //3.update accumulated_Depreciation in Fixed Asset
                var result = await _unitOfWork.FixedAsset.GetById((int)createDepreciationRegisterDto.FixedAssetId);
                if (result == null)
                    return new Response<FixedAssetDto>("Not found");
                result.SetAccumulatedDepreciationAmount(fixedAssetDto.AccumulatedDepreciationAmount + fixedAssetDto.DepreciableAmount);
            }
            //returning response
            return new Response<FixedAssetDto>(null, "Created successfully");

        }
        
        //Private methods
        private List<RemarksDto> ReturningRemarks(FixedAssetDto data)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.FixedAsset))
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

        private async Task AddToLedger(List<RecordLedger> recordLedgers = null)
        {
            var transaction = new Transactions(0, "", DocType.FixedAsset);
            var addTransaction = await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            // entity.SetTransactionId(transaction.Id);
            await _unitOfWork.SaveAsync();

            //Inserting data into recordledger table


            foreach (var item in recordLedgers)
            {
                item.SetTransactioId(transaction.Id);
            }



            await _unitOfWork.Ledger.AddRange(recordLedgers);
            await _unitOfWork.SaveAsync();
        }

    }
}
