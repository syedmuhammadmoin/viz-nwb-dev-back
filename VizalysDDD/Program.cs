
using Application.Contracts.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Seeds;
using Infrastructure.Uow;

var builder = WebApplication.CreateBuilder(args);

//Add database connection conf
builder.Services.AddInfrastructure(builder.Configuration);

// Add unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Add services
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<ICampusService, CampusService>();
builder.Services.AddScoped<IBusinessPartnerService, BusinessPartnerService>();
builder.Services.AddScoped<ILevel4Service, Level4Service>();
builder.Services.AddScoped<ILevel3Service, Level3Service>();
builder.Services.AddScoped<ICOAService, COAService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IJournalEntryService, JournalEntryService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<ICreditNoteService, CreditNoteService>();
builder.Services.AddScoped<IDebitNoteService, DebitNoteService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICashAccountService, CashAccountService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<IBankStmtService, BankStmtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBankReconService, BankReconService>();
builder.Services.AddScoped<IWorkFlowService, WorkFlowService>();
builder.Services.AddScoped<IWorkFlowStatusService, WorkFlowStatusService>();
builder.Services.AddScoped<ITransactionReconcileService, TransactionReconcileService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IGeneralLedgerReportService, GeneralLedgerReportService>();
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<AUTO GENERATED BY CONFLICT EXTENSION<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< develop
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<AUTO GENERATED BY CONFLICT EXTENSION<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< develop
builder.Services.AddScoped<ITrialBalanceReportService, TrialBalanceReportService>();
====================================AUTO GENERATED BY CONFLICT EXTENSION====================================
builder.Services.AddScoped<IBalanceSheetReportService, BalanceSheetReportService>();
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>AUTO GENERATED BY CONFLICT EXTENSION>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 2324-Add_Balance_Sheet_Report
====================================AUTO GENERATED BY CONFLICT EXTENSION====================================
builder.Services.AddScoped<IPNLReportService, PNLReportService>();
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>AUTO GENERATED BY CONFLICT EXTENSION>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 2351-Add_Profit_And_Loss_Report
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IRequisitionService, RequisitionService>();
builder.Services.AddScoped<IGRNService, GRNService>();

//Add auto mapper config
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//add HttpContextAccessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:4200",
                                "http://nwbtest.vizalys.com")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
        });
});

var app = builder.Build();
UserSeeding.Initialize(app.Services);
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
