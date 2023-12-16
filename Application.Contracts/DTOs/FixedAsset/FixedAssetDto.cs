using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class FixedAssetDto
    {



        //  private int _totalActiveDays;
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public DateTime DateofAcquisition { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int WarehouseId { get; set; }
        public string Warehouse { get; set; }
        public decimal SalvageValue { get; set; }
        public bool DepreciationApplicability { get; set; }
        public int? DepreciationModelId { get; set; }
        public string DepreciationModel { get; set; }
        public int? UseFullLife { get; set; }
        public Guid? AssetAccountId { get; set; }
        public string AssetAccount { get; set; }
        public Guid? DepreciationExpenseId { get; set; }
        public string DepreciationExpense { get; set; }
        public Guid? AccumulatedDepreciationId { get; set; }
        public string AccumulatedDepreciation { get; set; }
        public decimal AccumulatedDepreciationAmount { get; set; }
        public DepreciationMethod ModelType { get; set; }
        public decimal DecLiningRate { get; set; }
        public bool ProrataBasis { get; set; }
        public bool IsActive { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public bool IsHeldforSaleOrDisposal { get; set; }
        public bool IsIssued { get; set; }
        public bool IsReserved { get; set; }
        public bool IsDisposed { get; set; }
        public int DocId { get; set; }
        public DocType Doctype { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public int CampusId { get; set; }
        public string Campus { get; set; }
        public int TotalActiveDays { get; set; }
        public bool IsAllowedRole { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public List<FixedAssetLinesDto> FixedAssetlines { get; set; }
        public IEnumerable<DepreciationRegisterDto> DepriecaitonRegisterList { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string LastUser
        {
            get { return RemarksList?.LastOrDefault().UserName ?? ModifiedBy ?? CreatedBy; }
        }


        //---------------------------
        //Depreciation Related Stuff
        //---------------------------




        private bool IsSchedule { get; set; }
        private bool IsDepreciationConfigured { get; set; }
        private decimal RemainingDepreciationAmount => DepreciableAmount - AccumulatedDepreciationAmount;
        private decimal CalculatePerMonthDepreciation()
        {

            //if (UseFullLife == null || UseFullLife.Value==0)
            //{
            //    return 0;
            //}

            if (ModelType == DepreciationMethod.Declining)
            {
                return RemainingDepreciationAmount * DecLiningRate;
            }
            else if (ModelType == DepreciationMethod.StraightLine)
            {
                return DepreciableAmount / UseFullLife.Value;
            }


            // fault
            return 0;



        }
        private decimal PerMonthDepreciation => CalculatePerMonthDepreciation();
        private decimal PerDayDepreciation => PerMonthDepreciation / MonthDays;
        public decimal CalculateDepreciationAmount()
        {
            decimal _depreciationAmount;
            if (ProrataBasis)
            {
                _depreciationAmount = CalculateActiveDaysofMonth() * PerDayDepreciation;
            }
            else
            {
                _depreciationAmount = PerMonthDepreciation;
            }

            if (
                // To prevent under-depreciation and over-depreciation to the depreciable amount
                //fix: changed
                (AccumulatedDepreciationAmount + _depreciationAmount > DepreciableAmount)

                ||
                // Last Month of Depreciation 
                (DepreciationDate.Month == LastMonthofDepreciation && DepreciationDate.Year == LastYearofDepreciation)

                )
            {
                _depreciationAmount = DepreciableAmount - AccumulatedDepreciationAmount;
            }


            return _depreciationAmount;


        }
        private DateTime CalculateLastDepreciationDate()
        {
            const int numberOfMonths = 12;
            int usefulLifeInDay = YearDays / numberOfMonths * UseFullLife.Value;
            int remainingDays = usefulLifeInDay - (TotalActiveDays + CalculateActiveDaysofMonth());
            return DepreciationDate.AddDays(remainingDays);
        }
        private int LastMonthofDepreciation => CalculateLastDepreciationDate().Month;
        private int LastYearofDepreciation => CalculateLastDepreciationDate().Year;
        private enum YearDaysOption
        {
            ThreeSixty = 360,
            ThreeSixtyFive = 365

        }//360 or 365
        private int YearDays { get; set; } = (int)YearDaysOption.ThreeSixtyFive; //360 or 365
        private int MonthDays
        {
            get
            {
                // assumed that in else condition year_Days == 360
                return YearDays == 365 ? DateTime.DaysInMonth(DepreciationDate.Year, DepreciationDate.Month) : 30;
            }

        }
        private DateTime DepreciationDate { get; set; }
        private bool IsGoingtoDisposeAsset { get; set; }
        private bool IsAlreadyAutomatedDepreciatedforCurrentMonth()
        {
            if (this.DepriecaitonRegisterList != null)
            {
                return DepriecaitonRegisterList.Any(x => x.IsAutomatedCalculation == true);
            }
            return false;
        }
        private bool IsFirstMonthDepreciation()
        {

            if (DateofAcquisition.Year == DepreciationDate.Year && DateofAcquisition.Month == DepreciationDate.Month)
            {
                return true;
            }
            return false;
        }
        public decimal DepreciableAmount => Cost - SalvageValue;
        public int CalculateActiveDaysofMonth()
        {
            int _activeDaysofMonth = 0;
            int _currentDurationDays = 0;
            if (ProrataBasis)
            {
                const int currentDay = 1;

                if (IsSchedule && IsFirstMonthDepreciation())
                {
                    TimeSpan timeSpan = DepreciationDate- DateofAcquisition;
                    return timeSpan.Days + currentDay;
                }
                else
                if (IsSchedule)
                {
                    return MonthDays;
                }


                //fix: handle null active days record 
                //fix: suould take active Days by DeprecicationDate Month & Year  
                if (FixedAssetlines.Any(x => x.InactiveDate == null))
                {
                    TimeSpan timeSpan = DepreciationDate - FixedAssetlines.Where(x => x.InactiveDate == null).FirstOrDefault().ActiveDate;
                    _currentDurationDays = timeSpan.Days + currentDay;
                }

                _activeDaysofMonth = FixedAssetlines.Sum(x => x.ActiveDays) + _currentDurationDays;
                return _activeDaysofMonth;
            }
            else
            {
                if (IsSchedule)
                {
                    return MonthDays;
                }

                //Fix : changed
                if (FixedAssetlines.Count > 0)
                {
                    return MonthDays;
                }

            }
            return 0;
        }
        public void ConfigureDepreciation(DateTime depreciationDate, bool isGoingtoDisposeAsset, bool isSchedule)
        {
            DepreciationDate = depreciationDate;
            IsSchedule = isSchedule;
            IsGoingtoDisposeAsset = isGoingtoDisposeAsset;
            IsDepreciationConfigured = true;
        }
        public bool IsDepreciable
        {
            get
            {
                bool isDepreciationConfigured = IsDepreciationConfigured;
                bool isDepreciationApplicable = DepreciationApplicability;
                bool isActiveMonth = CalculateActiveDaysofMonth() >= 1;
                bool hasAmountToDepreciate = CalculateDepreciationAmount() >= 1;
                bool isAssetDisposed = IsDisposed;
                bool isAssetHeldForSaleOrDisposal = IsHeldforSaleOrDisposal;
                bool isGoingToDisposeAssetWithProrataBasisEnabled = (IsGoingtoDisposeAsset && ProrataBasis) || ProrataBasis;
                bool isNotAutomatedDepreciatedBefore = !IsAlreadyAutomatedDepreciatedforCurrentMonth();

                bool isDepreciable =
                    isDepreciationConfigured &&
                    isDepreciationApplicable &&
                    isActiveMonth &&
                    hasAmountToDepreciate &&
                    !isAssetDisposed &&
                    !isAssetHeldForSaleOrDisposal &&
                    isGoingToDisposeAssetWithProrataBasisEnabled &&
                    isNotAutomatedDepreciatedBefore;

                return isDepreciable;
            }



        }

    }
}
