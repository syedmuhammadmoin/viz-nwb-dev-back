using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class FixedAssetDto
    {

        private int _monthDay;
        private decimal _depreciationAmount;
        private decimal _perDayDepreciation;
        private decimal _perMonthDepreciation;
        private bool _isDepreciable;
        private decimal _depreciableAmount;
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
        public decimal PerMonthDepreciation
        {
            get
            {
                if (UseFullLife.Value==0 || UseFullLife==null)
                {
                    return 0;
                }

                if (ModelType == DepreciationMethod.Declining)
                {
                    return RemainingDepreciationAmount * DecLiningRate;
                }
                else if (ModelType == DepreciationMethod.StraightLine)
                {
                    return DepreciableAmount / UseFullLife.Value;
                }

                else
                {
                    // fault
                    return 0;
                }
            }
            set
            {



            }
        }
        public decimal PerDayDepreciation
        {
            get { return PerMonthDepreciation / MonthDays; }
            set
            {

            }
        }
        public decimal DepreciationAmount
        {
            get
            {
                _depreciationAmount = 0;
                if (ProrataBasis)
                {
                    _depreciationAmount = ActiveDaysofMonth() * PerDayDepreciation;
                }
                else
                {
                    _depreciationAmount = PerMonthDepreciation;
                }

                if (
                    // prevent to excessed Depreciation amount (monthly installment) to Depreciable Amount (Total Amount to Depreciate)
                    //fix: changed
                    (AccumulatedDepreciationAmount + _depreciationAmount > DepreciableAmount)

                    ||
                    // Last Month of Depreciation 
                    (CurrentDate.Month == LastMonthofDepreciation && CurrentDate.Year == LastYearofDepreciation)

                    )
                {
                    _depreciationAmount = DepreciableAmount - AccumulatedDepreciationAmount;
                }


                return _depreciationAmount;

            }
            set
            {
            }
        }
        public decimal DepreciableAmount
        {
            get { return _depreciableAmount = Cost - SalvageValue; }

        }
        public decimal RemainingDepreciationAmount
        {
            get
            {
                return DepreciableAmount - AccumulatedDepreciationAmount;
            }
        }
        public bool IsDepreciable
        {

            get
            {
                if (

                    

                   // Depreciation is disable
                   !DepreciationApplicability

                   ||
                    // whole month asset remain inactive
                    ActiveDaysofMonth() < 1

                   ||
                   // no any amount to depreciate
                   DepreciationAmount < 1

                   ||
                   // Asset is Disposed
                   IsDisposed

                   ||
                   // Asset is hold on Disposal
                   IsHeldforSaleOrDisposal

                   ||
                   // Going to Dispose and Prodata is disabled
                   (IsGoingtoDisposeAsset && ProrataBasis == false)

                   ||
                   // Already Depreciate Asset by Automatic Calcution
                   IsAlreadyAutomatedDepreciatedforCurrentMonth()

                   )
                {
                    _isDepreciable = false;
                    return _isDepreciable;
                }

                _isDepreciable = true;
                return _isDepreciable;


            }

            set { }

        }
        public int DepreciationMonth { get; set; }
        public int DepreciationYear { get; set; }
        public int LastMonthofDepreciation
        {
            get
            {
                int usefulLifeInDay = YearDays / 12 * UseFullLife.Value;
                int remainingDays = usefulLifeInDay - (TotalActiveDays + ActiveDaysofMonth());
                DateTime LastDayOfDepreciation = CurrentDate.AddDays(remainingDays);
                return LastDayOfDepreciation.Month;

            }
        }
        public int LastYearofDepreciation
        {
            get
            {
                int usefulLifeInDay = YearDays / 12 * UseFullLife.Value;
                int remainingDays = usefulLifeInDay - (TotalActiveDays + ActiveDaysofMonth());
                DateTime LastDayOfDepreciation = CurrentDate.AddDays(remainingDays);
                return LastDayOfDepreciation.Year;
            }
        }
        public int YearDays { get; set; } = 360; //360 or 365
        public int MonthDays
        {
            get
            {
                return  // assumed that in else condition year_Days == 360
              _monthDay = YearDays == 365 ? DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month) : 30;
            }
            set
            {

            }
        }
        public int TotalActiveDays { get; set; }
        public DateTime CurrentDate { get; set; } 
        public bool IsGoingtoDisposeAsset { get; set; }
        public bool IsAllowedRole { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public List<FixedAssetLinesDto> FixedAssetlines { get; set; }
        public IEnumerable<DepreciationRegisterDto> DepriecaitonRegisterList { get; set; }
        public bool IsAlreadyAutomatedDepreciatedforCurrentMonth()
        {
            if (this.DepriecaitonRegisterList != null)
            {
                return this.DepriecaitonRegisterList.Where(x => x.IsAutomatedCalculation == true).Count() > 0 ? true : false;
            }
            return false;
        }
        public int ActiveDaysofMonth()
        {
            int _activeDaysofMonth = 0;
            int _currentDurationDays = 0;
            if (ProrataBasis)
            {
                //fix: handle null active days record 
                if (FixedAssetlines.Where(x => x.InactiveDate == null).Count() > 0)
                {
                    TimeSpan timeSpan = CurrentDate - FixedAssetlines.Where(x => x.InactiveDate == null).FirstOrDefault().ActiveDate;
                    _currentDurationDays = timeSpan.Days + 1;
                }

                _activeDaysofMonth = FixedAssetlines.Sum(x => x.ActiveDays) + _currentDurationDays;
                return _activeDaysofMonth;
            }
            else
            {//Fix : changed
                if (FixedAssetlines.Count > 0)
                {
                    return MonthDays;
                }

            }
            return 0;
        }
        //public void CalculateLast_MonthAndYearofUseFul_Life()
        //{


        //    int usefulLifeInDay = YearDays / 12 * UseFullLife.Value;
        //    int remainingDays = usefulLifeInDay - TotalActiveDays + ActiveDaysofMonth();
        //    DateTime LastDayOfDepreciation = CurrentDate.AddDays(remainingDays);
        //    LastMonthofDepreciation = LastDayOfDepreciation.Month;
        //    LastYearofDepreciation = LastDayOfDepreciation.Year;

        //}


    }
}
