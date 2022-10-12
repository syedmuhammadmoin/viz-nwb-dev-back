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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GoodsReturnNoteService : IGoodsReturnNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GoodsReturnNoteService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getGRN = await _unitOfWork.GoodsReturnNote.GetById(data.DocId, new GoodsReturnNoteSpecs(true));

            if (getGRN == null)
            {
                return new Response<bool>("GRN with the input id not found");
            }
            if (getGRN.Status.State == DocumentStatus.Unpaid || getGRN.Status.State == DocumentStatus.Partial || getGRN.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("GRN already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.GoodsReturnNote)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getGRN.StatusId && x.Action == data.Action));

            if (transition == null)
            {
                return new Response<bool>("No transition found");
            }
            var getUser = new GetUser(this._httpContextAccessor);
            var userId = getUser.GetCurrentUserId();
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();
          
                foreach (var role in currentUserRoles)
                {
                    if (transition.AllowedRole.Name == role)
                    {
                        getGRN.setStatus(transition.NextStatusId);
                        if (!String.IsNullOrEmpty(data.Remarks))
                        {
                            var addRemarks = new Remark()
                            {
                                DocId = getGRN.Id,
                                DocType = DocType.GoodsReturnNote,
                                Remarks = data.Remarks,
                                UserId = userId
                            };
                            await _unitOfWork.Remarks.Add(addRemarks);
                        }
                        if (transition.NextStatus.State == DocumentStatus.Unpaid)
                        {
                            var reconciled = await ReconcileGRNLines(getGRN.Id, getGRN.GRNId, getGRN.GoodsReturnNoteLines);
                            if (!reconciled.IsSuccess)
                            {
                                _unitOfWork.Rollback();
                                return new Response<bool>(reconciled.Message);
                            }
                            await _unitOfWork.SaveAsync();

                            //Adding GRN Line in Stock
                            var removeItemsFromStock =  await RemoveItemInStock(getGRN);
                            if (!removeItemsFromStock.IsSuccess)
                            {
                                _unitOfWork.Rollback();
                                return new Response<bool>(removeItemsFromStock.Message);
                            }

                            _unitOfWork.Commit();
                            return new Response<bool>(true, "Goods Return Note Approved");
                        }
                        if (transition.NextStatus.State == DocumentStatus.Rejected)
                        {
                            await _unitOfWork.SaveAsync();
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "Goods Return Note Rejected");
                        }
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Goods Return Note Reviewed");
                    }
                }

                return new Response<bool>("User does not have allowed role");

          
        }

        public async Task<Response<GoodsReturnNoteDto>> CreateAsync(CreateGoodsReturnNoteDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitGoodsReturnNote(entity);
            }
            else
            {
                return await this.SaveGoodsReturnNote(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<GoodsReturnNoteDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var docDate = new List<DateTime?>();
            var states = new List<DocumentStatus?>();
            if (filter.DocDate != null)
            {
                docDate.Add(filter.DocDate);
            }
            if (filter.State != null)
            {
                states.Add(filter.State);
            }
            var goodsReturnNotes = await _unitOfWork.GoodsReturnNote.GetAll(new GoodsReturnNoteSpecs(docDate, states, filter, false));

            if (goodsReturnNotes.Count() == 0)
                return new PaginationResponse<List<GoodsReturnNoteDto>>(_mapper.Map<List<GoodsReturnNoteDto>>(goodsReturnNotes), "List is empty");

            var totalRecords = await _unitOfWork.GoodsReturnNote.TotalRecord(new GoodsReturnNoteSpecs(docDate, states, filter, true));

            return new PaginationResponse<List<GoodsReturnNoteDto>>(_mapper.Map<List<GoodsReturnNoteDto>>(goodsReturnNotes),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<GoodsReturnNoteDto>> GetByIdAsync(int id)
        {
            var specification = new GoodsReturnNoteSpecs(false);
            var goodsReturnNote = await _unitOfWork.GoodsReturnNote.GetById(id, specification);
            if (goodsReturnNote == null)
                return new Response<GoodsReturnNoteDto>("Not found");

            var goodsReturnNoteDto = _mapper.Map<GoodsReturnNoteDto>(goodsReturnNote);
            ReturningRemarks(goodsReturnNoteDto, DocType.GoodsReturnNote);
            ReturningFiles(goodsReturnNoteDto, DocType.GoodsReturnNote);
            goodsReturnNoteDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.GoodsReturnNote)).FirstOrDefault();


            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == goodsReturnNoteDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            goodsReturnNoteDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<GoodsReturnNoteDto>(goodsReturnNoteDto, "Returning value");

        }

        public async Task<Response<GoodsReturnNoteDto>> UpdateAsync(CreateGoodsReturnNoteDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitGoodsReturnNote(entity);
            }
            else
            {
                return await this.UpdateGoodsReturnNote(entity, 1);
            }
        }
        
        //Privte Methods for GoodsReturnNote

        private async Task<Response<GoodsReturnNoteDto>> SubmitGoodsReturnNote(CreateGoodsReturnNoteDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.GoodsReturnNote)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<GoodsReturnNoteDto>("No workflow found for GoodsReturnNote");
            }

            if (entity.Id == null)
            {
                return await this.SaveGoodsReturnNote(entity, 6);
            }
            else
            {
                return await this.UpdateGoodsReturnNote(entity, 6);
            }
        }

        private async Task<Response<GoodsReturnNoteDto>> SaveGoodsReturnNote(CreateGoodsReturnNoteDto entity, int status)
        {
            if (entity.GoodsReturnNoteLines.Count() == 0)
                return new Response<GoodsReturnNoteDto>("Lines are required");

            foreach (var goodsReturnNoteLine in entity.GoodsReturnNoteLines)
            {
                //Getting Unreconciled GoodsReturnNote lines
                var getGRNLine = _unitOfWork.GRN
                    .FindLines(new GRNLinesSpecs((int)goodsReturnNoteLine.ItemId, (int)goodsReturnNoteLine.WarehouseId, (int)entity.GRNId))
                    .FirstOrDefault();
                if (getGRNLine == null)
                    return new Response<GoodsReturnNoteDto>("No GoodsReturnNote line found for reconciliaiton");

                var checkValidation = CheckValidation((int)entity.GRNId, getGRNLine, _mapper.Map<GoodsReturnNoteLines>(goodsReturnNoteLine));
                if (!checkValidation.IsSuccess)
                    return new Response<GoodsReturnNoteDto>(checkValidation.Message);
            }

            //Checking duplicate Lines if any
            var duplicates = entity.GoodsReturnNoteLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<GoodsReturnNoteDto>("Duplicate Lines found");

            var goodsReturnNote = _mapper.Map<GoodsReturnNoteMaster>(entity);

            //Setting status
            goodsReturnNote.setStatus(status);

            _unitOfWork.CreateTransaction();
           
                //Saving in table
                var result = await _unitOfWork.GoodsReturnNote.Add(goodsReturnNote);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                goodsReturnNote.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<GoodsReturnNoteDto>(_mapper.Map<GoodsReturnNoteDto>(result), "Created successfully");
        
        }

        private async Task<Response<GoodsReturnNoteDto>> UpdateGoodsReturnNote(CreateGoodsReturnNoteDto entity, int status)
        {
            //setting GoodsReturnNoteId
            var getGRN = await _unitOfWork.GRN.GetById((int)entity.GRNId, new GRNSpecs(false));

            if (getGRN == null)
                return new Response<GoodsReturnNoteDto>("GRN not found");

            if (entity.GoodsReturnNoteLines.Count() == 0)
                return new Response<GoodsReturnNoteDto>("Lines are required");

            foreach (var goodsReturnNoteLine in entity.GoodsReturnNoteLines)
            {
                //Getting Unreconciled GoodsReturnNote lines
                var getGRNLine = _unitOfWork.GRN
                    .FindLines(new GRNLinesSpecs((int)goodsReturnNoteLine.ItemId, (int)goodsReturnNoteLine.WarehouseId, (int)entity.GRNId))
                    .FirstOrDefault();
                if (getGRNLine == null)
                    return new Response<GoodsReturnNoteDto>("No GoodsReturnNote line found for reconciliaiton");

                var checkValidation = CheckValidation((int)entity.GRNId, getGRNLine, _mapper.Map<GoodsReturnNoteLines>(goodsReturnNoteLine));
                if (!checkValidation.IsSuccess)
                    return new Response<GoodsReturnNoteDto>(checkValidation.Message);
            }

            //Checking duplicate Lines if any
            var duplicates = entity.GoodsReturnNoteLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<GoodsReturnNoteDto>("Duplicate Lines found");

            var specification = new GoodsReturnNoteSpecs(true);
            var gRN = await _unitOfWork.GoodsReturnNote.GetById((int)entity.Id, specification);

            if (gRN == null)
                return new Response<GoodsReturnNoteDto>("Not found");

            if (gRN.StatusId != 1 && gRN.StatusId != 2)
                return new Response<GoodsReturnNoteDto>("Only draft document can be edited");
            //Setting GoodsReturnNoteId
            gRN.setGRNId(getGRN.Id);

            //Setting status
            gRN.setStatus(status);

            _unitOfWork.CreateTransaction();
           
                //For updating data
                _mapper.Map<CreateGoodsReturnNoteDto, GoodsReturnNoteMaster>(entity, gRN);

                await _unitOfWork.SaveAsync();

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<GoodsReturnNoteDto>(_mapper.Map<GoodsReturnNoteDto>(gRN), "Updated successfully");
       
        }

        public Response<bool> CheckValidation(int grnId, GRNLines grnLine, GoodsReturnNoteLines goodsReturnNoteLine)
        {
            // Checking if given amount is greater than unreconciled document amount
            var reconciledGRNQty = _unitOfWork.GRNToGoodsReturnNoteLineReconcile
                .Find(new GRNToGoodsReturnNoteLineReconcileSpecs(grnId, grnLine.Id, grnLine.ItemId, grnLine.WarehouseId))
                .Sum(p => p.Quantity);

            var unreconciledGRNQty = grnLine.Quantity - reconciledGRNQty;

            if (goodsReturnNoteLine.Quantity > unreconciledGRNQty)
                return new Response<bool>("Enter quantity is greater than return quantity");

            return new Response<bool>(true, "No validation error found");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> ReconcileGRNLines(int goodsReturnNoteId, int grnId, List<GoodsReturnNoteLines> goodsReturnNoteLines)
        {
            foreach (var goodsReturnNoteLine in goodsReturnNoteLines)
            {
                //Getting Unreconciled Purchase Order lines
                var getGRNLine = _unitOfWork.GRN
                    .FindLines(new GRNLinesSpecs(goodsReturnNoteLine.ItemId, goodsReturnNoteLine.WarehouseId, grnId))
                    .FirstOrDefault();
                if (getGRNLine == null)
                    return new Response<bool>("No GRN line found for reconciliaiton");

                var checkValidation = CheckValidation(grnId, getGRNLine, goodsReturnNoteLine);
                if (!checkValidation.IsSuccess)
                    return new Response<bool>(checkValidation.Message);

                //Adding in Reconcilation table
                var recons = new GRNToGoodsReturnNoteLineReconcile(goodsReturnNoteLine.ItemId, goodsReturnNoteLine.Quantity,
                   goodsReturnNoteId, grnId, goodsReturnNoteLine.Id, getGRNLine.Id, goodsReturnNoteLine.WarehouseId);
                await _unitOfWork.GRNToGoodsReturnNoteLineReconcile.Add(recons);
                await _unitOfWork.SaveAsync();

                //Get total recon quantity
                var reconciledTotalGRNQty = _unitOfWork.GRNToGoodsReturnNoteLineReconcile
                    .Find(new GRNToGoodsReturnNoteLineReconcileSpecs(grnId, getGRNLine.Id, getGRNLine.ItemId, getGRNLine.WarehouseId))
                    .Sum(p => p.Quantity);

                // Updationg PO line status
                if (getGRNLine.Quantity == reconciledTotalGRNQty)
                {
                    getGRNLine.setStatus(DocumentStatus.Reconciled);
                }
                else
                {
                    getGRNLine.setStatus(DocumentStatus.Partial);
                }
                await _unitOfWork.SaveAsync();
            }

            //Update Purchase Order Master Status
            var getGRN = await _unitOfWork.GRN
                    .GetById(grnId, new GRNSpecs());

            var isGRNLinesReconciled = getGRN.GRNLines
                .Where(x => x.Status == DocumentStatus.Unreconciled || x.Status == DocumentStatus.Partial)
                .FirstOrDefault();

            if (isGRNLinesReconciled == null)
            {
                getGRN.setStatus(5);
            }
            else
            {
                getGRN.setStatus(4);
            }

            await _unitOfWork.SaveAsync();

            return new Response<bool>(true, "No validation error found");
        }

        public async Task<Response<bool>> RemoveItemInStock(GoodsReturnNoteMaster goodsReturnNote)
        {
            foreach (var line in goodsReturnNote.GoodsReturnNoteLines)
            {
                var getStockRecord = _unitOfWork.Stock.Find(new StockSpecs(line.ItemId, line.WarehouseId)).FirstOrDefault();

                if (getStockRecord == null)
                {
                    return new Response<bool>("Item not found in stock");
                }

                if (getStockRecord.AvailableQuantity >= line.Quantity)
                {
                    getStockRecord.updateAvailableQuantity(getStockRecord.AvailableQuantity - line.Quantity);

                }
                else
                {
                    return new Response<bool>("Item quantity is more than its availibility");
                }

                await _unitOfWork.SaveAsync();
            }
            return new Response<bool>(true, "");
        }
        private List<RemarksDto> ReturningRemarks(GoodsReturnNoteDto data, DocType docType)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.GoodsReturnNote))
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
        private List<FileUploadDto> ReturningFiles(GoodsReturnNoteDto data, DocType docType)
        {

            var files = _unitOfWork.Fileupload.Find(new FileUploadSpecs(data.Id, DocType.GoodsReturnNote))
                    .Select(e => new FileUploadDto()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DocType = DocType.GoodsReturnNote,
                        Extension = e.Extension,
                        UserName = e.User.UserName,
                        CreatedAt = e.CreatedDate == null ? "N/A" : ((DateTime)e.CreatedDate).ToString("ddd, dd MMM yyyy")
                    }).ToList();

            if (files.Count() > 0)
            {
                data.FileUploadList = _mapper.Map<List<FileUploadDto>>(files);

            }
            return files;


        }
    }
}
