using Application.Contracts.DTOs;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Application.Services;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Specifications;
using Infrastructure.Uow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.BackgroundServices
{
    public class DepreciationBackgroundService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _services;
       // private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public DepreciationBackgroundService(IServiceProvider services, IConfiguration configuration)
        {
            //_mapper = mapper;
            _services = services;
            _configuration = configuration;

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            var OgranzationDepreciationDateTime = _configuration["Organization:DepreciationScheduledDateTime"];
            var depreciationTime = Convert.ToDateTime(OgranzationDepreciationDateTime);

            int DepreciationMonth = DateTime.Now.Month;
            int DepreciationYear = DateTime.Now.Year;
            int DepreciationDay = depreciationTime.Day;// DateTime.DaysInMonth(DepreciationYear, DepreciationMonth); //DateTime.Now.Day; to run immediately
            int DepreciationHour = depreciationTime.Hour;
            int DepreciationMinute = depreciationTime.Minute;


            DateTime depreciationDate = new(DepreciationYear, DepreciationMonth, DepreciationDay, DepreciationHour, DepreciationMinute, 0);


            // Set the time for the first run at 3PM
            var now = DateTime.Now;
            
            var scheduledTime = depreciationDate;

            if (now > scheduledTime)
             {
                
                scheduledTime = scheduledTime.AddMonths(1);// Todo: add time to call next time AddSeconds(10); to run 10 second interval
                
            }

            TimeSpan delay = scheduledTime - now;
             

            // Set the timer to run the task at 3PM every day
            _timer = new Timer(DoWork, null, Convert.ToInt64(delay.TotalMilliseconds), Timeout.Infinite);
            return Task.CompletedTask;
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        

        public async Task<Response<FixedAssetDto>> GetByIdAsync(int id, int month, int year, UnitOfWork _unitOfWork, IMapper _mapper)
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetById(id, new FixedAssetSpecs());
            if (fixedAsset == null)
                return new Response<FixedAssetDto>("Not found");
            // var d = DateTime.Now;

            //Getting fixed asset lines
            var fixedAssetLines = await _unitOfWork.FixedAssetLines.GetByMonthAndYear(id, month, year);
            var fixedAssetLinesDto = _mapper.Map<List<FixedAssetLinesDto>>(fixedAssetLines);

            //Mappiing fixed asset
            var fixedAssetDto = _mapper.Map<FixedAssetDto>(fixedAsset);

            //Mapping Lines
            fixedAssetDto.FixedAssetlines = fixedAssetLinesDto;

            //Getting Depreciation register
            var depreciationRegister = await _unitOfWork.DepreciationRegister.GetByMonthAndYear(id, month, year);


            //Mappiing fixed asset
            var DepreciationRegisterDto = _mapper.Map<List<DepreciationRegisterDto>>(depreciationRegister);

            //Mapping Lines
            fixedAssetDto.DepriecaitonRegisterList = DepreciationRegisterDto;

            ReturningRemarks(fixedAssetDto, _unitOfWork, _mapper);


            if (fixedAssetDto.DepreciationApplicability == false)
            {
                fixedAssetDto.DepreciationModelId = null;
                fixedAssetDto.AssetAccountId = null;
                fixedAssetDto.AccumulatedDepreciation = null;
                fixedAssetDto.DepreciationExpenseId = null;
            }

            fixedAssetDto.IsAllowedRole = false;

           
            return new Response<FixedAssetDto>(fixedAssetDto, "Returning value");
        }
        private List<RemarksDto> ReturningRemarks(FixedAssetDto data, UnitOfWork _unitOfWork, IMapper _mapper)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.FixedAsset))
                    .Select(e => new RemarksDto()
                    {
                        Remarks = e.Remarks,
                        UserName = e.User.UserName,
                        CreatedAt = e.CreatedDate == null ? "N/A" : ((DateTime)e.CreatedDate).ToString("ddd, dd MMM yyyy")
                    }).ToList();

            if (remarks.Count > 0)
            {
                data.RemarksList = _mapper.Map<List<RemarksDto>>(remarks);
            }

            return remarks;
        }
        public async Task<Response<DepreciationRegisterDto>> CreateDepreciationRegisterAsync(CreateDepreciationRegisterDto entity, UnitOfWork _unitOfWork, IMapper _mapper)
        {

            var depreciationRegister = _mapper.Map<DepreciationRegister>(entity);
            await _unitOfWork.DepreciationRegister.Add(depreciationRegister);
            await _unitOfWork.SaveAsync();
            //returning response
            return new Response<DepreciationRegisterDto>(null, "Created successfully");

        }

        private async Task AddToLedger(List<RecordLedger> recordLedgers, UnitOfWork _unitOfWork)
        {
            var transaction = new Transactions(0, "", DocType.FixedAsset);
             await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            // entity.SetTransactionId(transaction.Id);
            //await _unitOfWork.SaveAsync();

            //Inserting data into recordledger table


            foreach (var item in recordLedgers)
            {
                item.SetTransactioId(transaction.Id);
            }



            await _unitOfWork.Ledger.AddRange(recordLedgers);
            await _unitOfWork.SaveAsync();
        }
        public enum Month
        {

            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }
        public async Task<Response<FixedAssetDto>> Depreciate(CreateDepreciationRegisterDto createDepreciationRegisterDto, UnitOfWork _unitOfWork, IMapper _mapper)
        {

            var fixedAsset = await GetByIdAsync(createDepreciationRegisterDto.FixedAssetId, createDepreciationRegisterDto.TransactionDate.Month, createDepreciationRegisterDto.TransactionDate.Year, _unitOfWork, _mapper);

            var fixedAssetDto = fixedAsset.Result;
            fixedAssetDto.ConfigureDepreciation(createDepreciationRegisterDto.TransactionDate, createDepreciationRegisterDto.IsGoingtoDispose, false);
            DateTime depreciationDate = createDepreciationRegisterDto.TransactionDate;
            decimal depreciationAmount = fixedAssetDto.CalculateDepreciationAmount();
            int activeDaysofMonth = fixedAssetDto.CalculateActiveDaysofMonth();
            _unitOfWork.CreateTransaction();

            try
            {
                if (createDepreciationRegisterDto.IsAutomatedCalculation)
                {
                    if (fixedAssetDto.IsDepreciable)
                    {
                        //1a. entry in Fixed Lines 

                        //Getting fixed asset lines
                        var fixedAssetLines = await _unitOfWork.FixedAssetLines.GetByMonthAndYear(fixedAssetDto.Id, depreciationDate.Month, depreciationDate.Year);

                        var maxActiveRecord = fixedAssetLines.OrderByDescending(r => r.ActiveDate).FirstOrDefault();

                        if (maxActiveRecord != null)
                        {
                            if (maxActiveRecord.InactiveDate == null)
                            {
                                maxActiveRecord.SetInactiveDate(depreciationDate);
                                TimeSpan timeSpan = maxActiveRecord.InactiveDate.Value - maxActiveRecord.ActiveDate;
                                maxActiveRecord.SetActiveDays(timeSpan.Days + 1); // add 1 to include both start and end dates
                                var createFixedAssetlineDto = _mapper.Map<FixedAssetLines>(maxActiveRecord);
                                await CreateFixedAssetLinesAsync(createFixedAssetlineDto, _unitOfWork);
                            }
                        }


                        // 1b. add new month active date
                        // but No need in case of Held for Disposal 
                        if (createDepreciationRegisterDto.IsGoingtoDispose==false)
                        {
                            var createFixedAssetlineDto2 = new FixedAssetLinesDto() { ActiveDate = depreciationDate.AddDays(1), MasterId = fixedAssetDto.Id };
                            await CreateFixedAssetLinesAsync(_mapper.Map<FixedAssetLines>(createFixedAssetlineDto2), _unitOfWork);

                        }
                        // 2.entry in Depreciation Register						
                        createDepreciationRegisterDto.Description = "Depreciation of month " + depreciationDate.Month.ToString() + " / " + depreciationDate.Year.ToString();
                        createDepreciationRegisterDto.DepreciationAmount = fixedAssetDto.CalculateDepreciationAmount();
                        await CreateDepreciationRegisterAsync(createDepreciationRegisterDto, _unitOfWork, _mapper);


                        // 3.entry in the ledger
                        List<RecordLedger> recordLedgers = new List<RecordLedger>() {
                    new RecordLedger(0, fixedAssetDto.AccumulatedDepreciationId, null, fixedAssetDto.WarehouseId, "Asset" + fixedAssetDto.Id, 'C',depreciationAmount, null, depreciationDate,fixedAssetDto.Id),
                    new RecordLedger(0, fixedAssetDto.DepreciationExpenseId, null, fixedAssetDto.WarehouseId, "Asset" + fixedAssetDto.Id, 'D', depreciationAmount, null, depreciationDate,fixedAssetDto.Id)
                    };

                        await AddToLedger(recordLedgers, _unitOfWork);

                        // 4.update accumulated_Depreciation in Fixed Asset	
                        var result = await _unitOfWork.FixedAsset.GetById((int)createDepreciationRegisterDto.FixedAssetId);
                        if (result == null)
                            return new Response<FixedAssetDto>("Not found");
                        result.SetAccumulatedDepreciationAmount(fixedAssetDto.AccumulatedDepreciationAmount + depreciationAmount);
                        //5.Update Active Days in FixedAsset 
                        result.SetTotalActiveDays(fixedAssetDto.TotalActiveDays + activeDaysofMonth);




                        await _unitOfWork.SaveAsync();


                    }
                    else
                    {

                        //Create log // due to working in background 
                    }

                }



                //Commiting the transaction
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {

                _unitOfWork.Rollback();
                return new Response<FixedAssetDto>(e.Message);
            }
            //returning response
            return new Response<FixedAssetDto>(null, "Created successfully");

        }

       
        private async void DoWork(object state)
        {
            using var scope = _services.CreateScope();
            bool executeForTestingPurpose = false;

            if (executeForTestingPurpose)
            {
                List<DateTime> dateTimes = new List<DateTime>();
                int numberofMonth = 12;
                int DepreciationMonth = (int)Month.January;
                int DepreciationYear = 2020;
                int DepreciationDay = DateTime.DaysInMonth(DepreciationYear, DepreciationMonth);
                DateTime depreciationDate = new DateTime(DepreciationYear, DepreciationMonth, DepreciationDay); // DateTime.Now;

                dateTimes.Add(depreciationDate);


                for (int i = 1; i < numberofMonth; i++)
                {
                    dateTimes.Add(depreciationDate.AddMonths(i));

                }
                foreach (var date in dateTimes)
                {

                    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    var mapper = scope.ServiceProvider.GetService<IMapper>();
                    var unitOfWork = new UnitOfWork(dbContext,null);

                    var createDepreciationRegisterDto = dbContext.FixedAssets
                        .Where(i => i.IsDisposed == false && i.IsHeldforSaleOrDisposal == false
                        && i.DepreciationApplicability == true)
                        .Select(i => new CreateDepreciationRegisterDto
                        {
                            FixedAssetId = i.Id,
                            TransactionDate = date,
                            IsAutomatedCalculation = true,
                            IsGoingtoDispose = false
                        }).ToList();

                    foreach (var item in createDepreciationRegisterDto)
                    {
                        await Depreciate(item, unitOfWork, mapper);
                    }
                }
            }
            else
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                var unitOfWork = new UnitOfWork(dbContext, null);

                var createDepreciationRegisterDto = dbContext.FixedAssets
                    .Where(i => i.IsDisposed == false && i.IsHeldforSaleOrDisposal == false
                    && i.DepreciationApplicability == true)
                    .Select(i => new CreateDepreciationRegisterDto
                    {
                        FixedAssetId = i.Id,
                        TransactionDate = DateTime.Now,
                        IsAutomatedCalculation = true,
                        IsGoingtoDispose = false
                    }).ToList();

                foreach (var item in createDepreciationRegisterDto)
                {
                    await Depreciate(item, unitOfWork, mapper);
                }
            }
        }

        public async Task<Response<FixedAssetLinesDto>> CreateFixedAssetLinesAsync(FixedAssetLines entity, UnitOfWork _unitOfWork)
        {
            if (entity.Id == 0)
            {
                await _unitOfWork.FixedAssetLines.Add(entity);
            }


            await _unitOfWork.SaveAsync();
            //returning response
            return new Response<FixedAssetLinesDto>(null, "Created successfully");

        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
