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
            level1.Add(new Level1(new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00"), "Assets", orgId));
            level1.Add(new Level1(new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00"), "Liability", orgId));
            level1.Add(new Level1(new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00"), "Equity", orgId));
            level1.Add(new Level1(new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00"), "Income", orgId));
            level1.Add(new Level1(new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"), "Expenses", orgId));
            await unitOfWork.Level1.AddRange(level1);

            var level2 = new List<Level2>();
            level2.Add(new Level2(new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"), "Non - Current Assets", new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level2.Add(new Level2(new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"), "Current Assets", new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level2.Add(new Level2(new Guid("21000000-5566-7788-99AA-BBCCDDEEFF00"), "Non - Current Liabilities", new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level2.Add(new Level2(new Guid("22000000-5566-7788-99AA-BBCCDDEEFF00"), "Current Liabilities", new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level2.Add(new Level2(new Guid("31000000-5566-7788-99AA-BBCCDDEEFF00"), "Owner's Equity", new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level2.Add(new Level2(new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"), "Trading Income", new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level2.Add(new Level2(new Guid("42000000-5566-7788-99AA-BBCCDDEEFF00"), "Non - Trading Income", new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level2.Add(new Level2(new Guid("51000000-5566-7788-99AA-BBCCDDEEFF00"), "Cost of Goods Sold", new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level2.Add(new Level2(new Guid("52000000-5566-7788-99AA-BBCCDDEEFF00"), "General Expenses", new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            await unitOfWork.Level2.AddRange(level2);

            var level3 = new List<Level3>();
            level3.Add(new Level3(new Guid("11100000-5566-7788-99AA-BBCCDDEEFF00"), "Fixed Assets", new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00"), "Receivable", new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00"), "Bank & Cash", new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00"), "Prepayment", new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("12400000-5566-7788-99AA-BBCCDDEEFF00"), "Inventory / Merchandise", new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("12500000-5566-7788-99AA-BBCCDDEEFF00"), "Other Current Asset", new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("21100000-5566-7788-99AA-BBCCDDEEFF00"), "Loans Payable", new Guid("21000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("21200000-5566-7788-99AA-BBCCDDEEFF00"), "Other Non - Current Liability", new Guid("21000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"), "Payable", new Guid("22000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("22200000-5566-7788-99AA-BBCCDDEEFF00"), "Short Term Credit", new Guid("22000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("22300000-5566-7788-99AA-BBCCDDEEFF00"), "Other Current Liability", new Guid("22000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("31100000-5566-7788-99AA-BBCCDDEEFF00"), "Capital", new Guid("31000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("31200000-5566-7788-99AA-BBCCDDEEFF00"), "Reserves", new Guid("31000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("41100000-5566-7788-99AA-BBCCDDEEFF00"), "Revenue", new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("42100000-5566-7788-99AA-BBCCDDEEFF00"), "Other Income", new Guid("42000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("51100000-5566-7788-99AA-BBCCDDEEFF00"), "Cost of Revenue", new Guid("51000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("51200000-5566-7788-99AA-BBCCDDEEFF00"), "Depreciation", new Guid("51000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"), "Administrative Expenses", new Guid("52000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("52200000-5566-7788-99AA-BBCCDDEEFF00"), "Finance Charge", new Guid("52000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("52300000-5566-7788-99AA-BBCCDDEEFF00"), "Tax Expenses", new Guid("52000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            level3.Add(new Level3(new Guid("52400000-5566-7788-99AA-BBCCDDEEFF00"), "Selling Expense", new Guid("52000000-5566-7788-99AA-BBCCDDEEFF00"), orgId));
            await unitOfWork.Level3.AddRange(level3);

            var level4 = new List<Level4>();
            level4.Add(new Level4(new Guid("31210000-5566-7788-99AA-BBCCDDEEFF00"), "Opening Balance Equity", new Guid("31200000-5566-7788-99AA-BBCCDDEEFF00"), new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00" ), orgId));
            level4.Add(new Level4(new Guid("12510000-5566-7788-99AA-BBCCDDEEFF00"), "Sales Tax Asset", new Guid("12500000-5566-7788-99AA-BBCCDDEEFF00"), new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00" ), orgId));
            level4.Add(new Level4(new Guid("12520000-5566-7788-99AA-BBCCDDEEFF00"), "Income Tax Asset", new Guid("12500000-5566-7788-99AA-BBCCDDEEFF00"), new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00" ), orgId));
            level4.Add(new Level4(new Guid("22310000-5566-7788-99AA-BBCCDDEEFF00"), "Sales Tax Liability", new Guid("22300000-5566-7788-99AA-BBCCDDEEFF00"), new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00" ), orgId));
            level4.Add(new Level4(new Guid("22320000-5566-7788-99AA-BBCCDDEEFF00"), "Income Tax Liability", new Guid("22300000-5566-7788-99AA-BBCCDDEEFF00"), new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00" ), orgId));
            level4.Add(new Level4(new Guid("42110000-5566-7788-99AA-BBCCDDEEFF00"), "Discount Allowed", new Guid("42100000-5566-7788-99AA-BBCCDDEEFF00"), new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00" ), orgId));
            level4.Add(new Level4(new Guid("42120000-5566-7788-99AA-BBCCDDEEFF00"), "Discount Received", new Guid("42100000-5566-7788-99AA-BBCCDDEEFF00"), new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00" ), orgId));
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
