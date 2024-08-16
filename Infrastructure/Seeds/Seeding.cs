using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Seeds
{
    public static class Seeding
    {


        public static async Task COASeeds(IUnitOfWork unitOfWork, int orgId)
        {
            await AddLevel1(unitOfWork, orgId);
            await AddLevel2(unitOfWork, orgId);
            await AddLevel3(unitOfWork, orgId);
            await AddLevel4(unitOfWork, orgId);

            await unitOfWork.SaveAsync();
        }

        private static async Task AddLevel1(IUnitOfWork unitOfWork, int orgId)
        {
            var level1 = new List<Level1>
    {
        new Level1(GenerateId("10000000", orgId), "Assets", orgId),
        new Level1(GenerateId("20000000", orgId), "Liability", orgId),
        new Level1(GenerateId("30000000", orgId), "Equity", orgId),
        new Level1(GenerateId("40000000", orgId), "Income", orgId),
        new Level1(GenerateId("50000000", orgId), "Expenses", orgId),
        new Level1(GenerateId("60000000", orgId), "Other", orgId)
    };
            await unitOfWork.Level1.AddRange(level1);
        }

        private static async Task AddLevel2(IUnitOfWork unitOfWork, int orgId)
        {
            var level2 = new List<Level2>
    {
        // Assets
        new Level2(GenerateId("11000000", orgId), "Receivable", GenerateId("10000000", orgId), orgId),
        new Level2(GenerateId("12000000", orgId), "Bank & Cash", GenerateId("10000000", orgId), orgId),
        new Level2(GenerateId("13000000", orgId), "Current Assets", GenerateId("10000000", orgId), orgId),
        new Level2(GenerateId("14000000", orgId), "Non - Current Assets", GenerateId("10000000", orgId), orgId),
        new Level2(GenerateId("15000000", orgId), "Prepayments", GenerateId("10000000", orgId), orgId),
        new Level2(GenerateId("16000000", orgId), "Fixed Assets", GenerateId("10000000", orgId), orgId),
        // Liability
        new Level2(GenerateId("21000000", orgId), "Payable", GenerateId("20000000", orgId), orgId),
        new Level2(GenerateId("22000000", orgId), "Credit Card", GenerateId("20000000", orgId), orgId),
        new Level2(GenerateId("23000000", orgId), "Current Liabilities", GenerateId("20000000", orgId), orgId),
        new Level2(GenerateId("24000000", orgId), "Non - Current Liabilities", GenerateId("20000000", orgId), orgId),
        // Equity
        new Level2(GenerateId("31000000", orgId), "Equity", GenerateId("30000000", orgId), orgId),
        new Level2(GenerateId("32000000", orgId), "Current year Earnings", GenerateId("30000000", orgId), orgId),
        // Income
        new Level2(GenerateId("41000000", orgId), "Income", GenerateId("40000000", orgId), orgId),
        new Level2(GenerateId("42000000", orgId), "Other Income", GenerateId("40000000", orgId), orgId),
        // Expenses
        new Level2(GenerateId("51000000", orgId), "Expenses", GenerateId("50000000", orgId), orgId),
        new Level2(GenerateId("52000000", orgId), "Depreciation", GenerateId("50000000", orgId), orgId),
        new Level2(GenerateId("53000000", orgId), "Cost of Revenue", GenerateId("50000000", orgId), orgId),
        // Other
        new Level2(GenerateId("61000000", orgId), "Off-Balance Sheet", GenerateId("60000000", orgId), orgId)
    };
            await unitOfWork.Level2.AddRange(level2);
        }

        private static async Task AddLevel3(IUnitOfWork unitOfWork, int orgId)
        {
            var level3 = new List<Level3>
    {
        // Assets
        new Level3(GenerateId("11100000", orgId), "Receivable", GenerateId("11000000", orgId), orgId),
        new Level3(GenerateId("12100000", orgId), "Bank & Cash", GenerateId("12000000", orgId), orgId),
        new Level3(GenerateId("13100000", orgId), "Current Assets", GenerateId("13000000", orgId), orgId),
        new Level3(GenerateId("14100000", orgId), "Non - Current Assets", GenerateId("14000000", orgId), orgId),
        new Level3(GenerateId("15100000", orgId), "Prepayments", GenerateId("15000000", orgId), orgId),
        new Level3(GenerateId("16100000", orgId), "Fixed Assets", GenerateId("16000000", orgId), orgId),
        // Liability
        new Level3(GenerateId("21100000", orgId), "Payable", GenerateId("21000000", orgId), orgId),
        new Level3(GenerateId("22100000", orgId), "Credit Card", GenerateId("22000000", orgId), orgId),
        new Level3(GenerateId("23100000", orgId), "Current Liabilities", GenerateId("23000000", orgId), orgId),
        new Level3(GenerateId("24100000", orgId), "Non - Current Liabilities", GenerateId("24000000", orgId), orgId),
        // Equity
        new Level3(GenerateId("31100000", orgId), "Equity", GenerateId("31000000", orgId), orgId),
        new Level3(GenerateId("32100000", orgId), "Current year Earnings", GenerateId("32000000", orgId), orgId),
        // Income
        new Level3(GenerateId("41100000", orgId), "Income", GenerateId("41000000", orgId), orgId),
        new Level3(GenerateId("42100000", orgId), "Other Income", GenerateId("42000000", orgId), orgId),
        // Expenses
        new Level3(GenerateId("51100000", orgId), "Expenses", GenerateId("51000000", orgId), orgId),
        new Level3(GenerateId("52100000", orgId), "Depreciation", GenerateId("52000000", orgId), orgId),
        new Level3(GenerateId("53100000", orgId), "Cost of Revenue", GenerateId("53000000", orgId), orgId),
        // Other
        new Level3(GenerateId("61100000", orgId), "Off-Balance Sheet", GenerateId("61000000", orgId), orgId)
    };
            await unitOfWork.Level3.AddRange(level3);
        }

        private static async Task AddLevel4(IUnitOfWork unitOfWork, int orgId)
        {
            var level4 = new List<Level4>
    {
            //Asset > Bank & Cash
            new Level4(GenerateId("12110000", orgId), "Bank", GenerateId("12100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("12120000", orgId), "Cash", GenerateId("12100000", orgId), GenerateId("10000000", orgId), orgId),
    
            //Asset > Current Assets
            new Level4(GenerateId("13110000", orgId), "Bank Suspense Account", GenerateId("13100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("13120000", orgId), "Cost of Production", GenerateId("13100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("13130000", orgId), "Current Assets", GenerateId("13100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("13140000", orgId), "Liquidity Transfer", GenerateId("13100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("13150000", orgId), "Outstanding Payments", GenerateId("13100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("13160000", orgId), "Outstanding Receipts", GenerateId("13100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("13170000", orgId), "Prepaid Expenses", GenerateId("13100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("13180000", orgId), "Products to receive", GenerateId("13100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("13190000", orgId), "Stock Interim (Delivered)", GenerateId("13100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("131100000", orgId), "Stock Interim (Received)", GenerateId("13100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("131110000", orgId), "Stock Valuation", GenerateId("13100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("131120000", orgId), "Tax Paid", GenerateId("13100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("131130000", orgId), "Tax Receivable", GenerateId("13100000", orgId), GenerateId("10000000", orgId), orgId),
                
            //Asset > Fixed Assets
            new Level4(GenerateId("16110000", orgId), "Fixed Assets", GenerateId("16100000", orgId), GenerateId("10000000", orgId), orgId),

            //Asset > Non - Current Assets
            new Level4(GenerateId("14110000", orgId), "Non - Current Assets", GenerateId("14100000", orgId), GenerateId("10000000", orgId), orgId),
                
            //Asset >  Prepayments 
            new Level4(GenerateId("15110000", orgId), "Prepayments", GenerateId("15100000", orgId), GenerateId("10000000", orgId), orgId),

            //Asset >  Receivable 
            new Level4(GenerateId("11110000", orgId), "Account Receivable (PoS)", GenerateId("11100000", orgId), GenerateId("10000000", orgId), orgId),
            new Level4(GenerateId("11112000", orgId), "Account Receivable", GenerateId("11100000", orgId), GenerateId("10000000", orgId), orgId),


            //Equity > Equity 
            new Level4(GenerateId("31110000", orgId), "Capital", GenerateId("31100000", orgId), GenerateId("30000000", orgId), orgId),
            new Level4(GenerateId("31120000", orgId), "Dividends", GenerateId("31100000", orgId), GenerateId("30000000", orgId), orgId),

            //Equity > Current Year Earnings
            new Level4(GenerateId("32110000", orgId), "Undistributed Profits/Losses", GenerateId("32100000", orgId), GenerateId("30000000", orgId), orgId),

            //Expenses > Expenses
            new Level4(GenerateId("51110000", orgId), "Cash Discount Loss", GenerateId("51100000", orgId), GenerateId("50000000", orgId), orgId),
            new Level4(GenerateId("51120000", orgId), "Expenses", GenerateId("51100000", orgId), GenerateId("50000000", orgId), orgId),
            new Level4(GenerateId("51130000", orgId), "Purchase of Equipments", GenerateId("51100000", orgId), GenerateId("50000000", orgId), orgId),
            new Level4(GenerateId("51140000", orgId), "Rent", GenerateId("51100000", orgId), GenerateId("50000000", orgId), orgId),
            new Level4(GenerateId("51150000", orgId), "Bank Fees", GenerateId("51100000", orgId), GenerateId("50000000", orgId), orgId),
            new Level4(GenerateId("51160000", orgId), "Salary Expenses", GenerateId("51100000", orgId), GenerateId("50000000", orgId), orgId),
            new Level4(GenerateId("51170000", orgId), "Foreign Exchange Loss", GenerateId("51100000", orgId), GenerateId("50000000", orgId), orgId),
            new Level4(GenerateId("51180000", orgId), "Cash Difference Loss", GenerateId("51100000", orgId), GenerateId("50000000", orgId), orgId),
            new Level4(GenerateId("51190000", orgId), "RD Expenses", GenerateId("51100000", orgId), GenerateId("50000000", orgId), orgId),
            new Level4(GenerateId("511100000", orgId), "Sales Expenses", GenerateId("51100000", orgId), GenerateId("50000000", orgId), orgId),

            //Expenses > Cost of Revenue
            new Level4(GenerateId("53110000", orgId), "Cost of Goods Sold", GenerateId("53100000", orgId), GenerateId("50000000", orgId), orgId),
                 
            //Income > Income
            new Level4(GenerateId("41110000", orgId), "Product Sales", GenerateId("41100000", orgId), GenerateId("40000000", orgId), orgId),
            new Level4(GenerateId("41120000", orgId), "Foreign Exchange Gain", GenerateId("41100000", orgId), GenerateId("40000000", orgId), orgId),
            new Level4(GenerateId("41130000", orgId), "Cash Difference Gain", GenerateId("41100000", orgId), GenerateId("40000000", orgId), orgId),
            new Level4(GenerateId("41140000", orgId), "Cash Discount Gain", GenerateId("41100000", orgId), GenerateId("40000000", orgId), orgId),
                
            //Income > Other Income
            new Level4(GenerateId("42110000", orgId), "Other Income", GenerateId("42100000", orgId), GenerateId("40000000", orgId), orgId),
                
                // Liability > Current Liabilities
            new Level4(GenerateId("23110000", orgId), "Current Liabilities", GenerateId("23100000", orgId), GenerateId("20000000", orgId), orgId),
            new Level4(GenerateId("23120000", orgId), "Bills to receive", GenerateId("23100000", orgId), GenerateId("20000000", orgId), orgId),
            new Level4(GenerateId("23130000", orgId), "Deferred Revenue", GenerateId("23100000", orgId), GenerateId("20000000", orgId), orgId),
            new Level4(GenerateId("23140000", orgId), "Salary Payable", GenerateId("23100000", orgId), GenerateId("20000000", orgId), orgId),
            new Level4(GenerateId("23150000", orgId), "Employee Payroll Taxes", GenerateId("23100000", orgId), GenerateId("20000000", orgId), orgId),
            new Level4(GenerateId("23160000", orgId), "Employer Payroll Taxes", GenerateId("23100000", orgId), GenerateId("20000000", orgId), orgId),
            new Level4(GenerateId("23170000", orgId), "Tax Received", GenerateId("23100000", orgId), GenerateId("20000000", orgId), orgId),
            new Level4(GenerateId("23180000", orgId), "Tax Payable", GenerateId("23100000", orgId), GenerateId("20000000", orgId), orgId),

                // Liability > Non-current Liabilities
            new Level4(GenerateId("24110000", orgId), "Non-current Liabilities", GenerateId("24100000", orgId), GenerateId("20000000", orgId), orgId),

                // Liability > Payable
            new Level4(GenerateId("21110000", orgId), "Account Payable", GenerateId("24100000", orgId), GenerateId("20000000", orgId), orgId),
    };
            await unitOfWork.Level4.AddRange(level4);
        }

        private static string GenerateId(string baseId, int orgId)
        {
            return $"{baseId}-5566-7788-99AA-BBCCDDEEFF00-{orgId}";
        }

        public static void seeds(ModelBuilder modelBuilder)
        {
            //Adding seeds in organization table
            //modelBuilder.Entity<Organization>()
            //    .HasData(
            //        new Organization(1, "SBBU")
            //    );

            //Adding seeds in workflow status
            modelBuilder.Entity<WorkFlowStatus>()
                .HasData(
                    new WorkFlowStatus(1, "Draft", DocumentStatus.Draft, StatusType.PreDefined),
                    new WorkFlowStatus(2, "Rejected", DocumentStatus.Rejected, StatusType.PreDefinedInList),
                    new WorkFlowStatus(3, "Unpaid", DocumentStatus.Unpaid, StatusType.PreDefined),
                    new WorkFlowStatus(4, "Partial Paid", DocumentStatus.Partial, StatusType.PreDefined),
                    new WorkFlowStatus(5, "Paid", DocumentStatus.Paid, StatusType.PreDefined),
                    new WorkFlowStatus(6, "Submitted", DocumentStatus.Submitted, StatusType.PreDefinedInList),
                    new WorkFlowStatus(7, "Cancelled", DocumentStatus.Cancelled, StatusType.PreDefined)
                );







            //SBBU-Code
            //Adding seeds in TaxAccounts
            //modelBuilder.Entity<Taxes>()
            //    .HasData(
            //        new Taxes(1, "Sales Tax Asset", TaxType.SalesTaxAsset),
            //        new Taxes(2, "Sales Tax Liability", TaxType.SalesTaxLiability),
            //        new Taxes(3, "Income Tax Asset", TaxType.IncomeTaxAsset),
            //        new Taxes(4, "Income Tax Liability", TaxType.IncomeTaxLiability),
            //        new Taxes(5, "SRB Tax Asset", TaxType.SRBTaxAsset),
            //        new Taxes(6, "SRB Tax Liability", TaxType.SRBTaxLiability)
            //        );
        }
    }
}
