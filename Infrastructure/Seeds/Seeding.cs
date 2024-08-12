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
            var level1 = new List<Level1>();
            level1.Add(new Level1("10000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Assets", orgId));
            level1.Add(new Level1("20000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Liability", orgId));
            level1.Add(new Level1("30000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Equity", orgId));
            level1.Add(new Level1("40000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Income", orgId));
            level1.Add(new Level1("50000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Expenses", orgId));
            await unitOfWork.Level1.AddRange(level1);

            var level2 = new List<Level2>();
            level2.Add(new Level2("11000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Non - Current Assets", "10000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level2.Add(new Level2("12000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Current Assets", "10000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level2.Add(new Level2("21000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Non - Current Liabilities", "20000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level2.Add(new Level2("22000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Current Liabilities", "20000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level2.Add(new Level2("31000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Owner's Equity", "30000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level2.Add(new Level2("41000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Trading Income", "40000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level2.Add(new Level2("42000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Non - Trading Income", "40000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level2.Add(new Level2("51000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Cost of Goods Sold", "50000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level2.Add(new Level2("52000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "General Expenses", "50000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            await unitOfWork.Level2.AddRange(level2);

            var level3 = new List<Level3>();
            level3.Add(new Level3("11100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Fixed Assets", "11000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("12100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Receivable", "12000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("12200000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Bank & Cash", "12000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("12300000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Prepayment", "12000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("12400000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Inventory / Merchandise", "12000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("12500000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Other Current Asset", "12000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("21100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Loans Payable", "21000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("21200000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Other Non - Current Liability", "21000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("22100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Payable", "22000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("22200000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Short Term Credit", "22000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("22300000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Other Current Liability", "22000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("31100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Capital", "31000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("31200000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Reserves", "31000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("41100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Revenue", "41000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("42100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Other Income", "42000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("51100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Cost of Revenue", "51000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("51200000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Depreciation", "51000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("52100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Administrative Expenses", "52000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("52200000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Finance Charge", "52000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("52300000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Tax Expenses", "52000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level3.Add(new Level3("52400000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Selling Expense", "52000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            await unitOfWork.Level3.AddRange(level3);

            var level4 = new List<Level4>();
            level4.Add(new Level4("31210000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Opening Balance Equity", "31200000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "30000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level4.Add(new Level4("12510000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Sales Tax Asset", "12500000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "10000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level4.Add(new Level4("12520000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Income Tax Asset", "12500000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "10000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level4.Add(new Level4("22310000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Sales Tax Liability", "22300000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "20000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level4.Add(new Level4("22320000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Income Tax Liability", "22300000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "20000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level4.Add(new Level4("42110000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Discount Allowed", "42100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "40000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            level4.Add(new Level4("42120000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "Discount Received", "42100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", "40000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}", orgId));
            await unitOfWork.Level4.AddRange(level4);

            await unitOfWork.SaveAsync();
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








            //Adding seeds in TaxAccounts
            modelBuilder.Entity<Taxes>()
                .HasData(
                    new Taxes(1, "Sales Tax Asset", TaxType.SalesTaxAsset),
                    new Taxes(2, "Sales Tax Liability", TaxType.SalesTaxLiability),
                    new Taxes(3, "Income Tax Asset", TaxType.IncomeTaxAsset),
                    new Taxes(4, "Income Tax Liability", TaxType.IncomeTaxLiability),
                    new Taxes(5, "SRB Tax Asset", TaxType.SRBTaxAsset),
                    new Taxes(6, "SRB Tax Liability", TaxType.SRBTaxLiability)
                    );
        }
    }
}
