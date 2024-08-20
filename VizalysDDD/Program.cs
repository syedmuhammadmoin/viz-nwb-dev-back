using Application.BackgroundServices;
using Application.Contracts.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.GlobalExceptionFilter;
using Infrastructure.Seeds;
using Infrastructure.Uow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

//Add database connection conf
builder.Services.AddInfrastructure(builder.Configuration);

// Add unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Add services
builder.Services.AddScoped(provider => new Lazy<IEmployeeService>(() => provider.GetRequiredService<IEmployeeService>()));
builder.Services.AddScoped(provider => new Lazy<IWarehouseService>(() => provider.GetRequiredService<IWarehouseService>()));
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
builder.Services.AddScoped<IPettyCashService, PettyCashService>();
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
builder.Services.AddScoped<ITrialBalanceReportService, TrialBalanceReportService>();
builder.Services.AddScoped<IBalanceSheetReportService, BalanceSheetReportService>();
builder.Services.AddScoped<IPNLReportService, PNLReportService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IRequisitionService, RequisitionService>();
builder.Services.AddScoped<IGRNService, GRNService>();
builder.Services.AddScoped<IEstimatedBudgetService, EstimatedBudgetService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPayrollItemService, PayrollItemService>();
builder.Services.AddScoped<IPayrollTransactionService, PayrollTransactionService>();
builder.Services.AddScoped<ITaxService, TaxService>();
builder.Services.AddScoped<IUnitOfMeasurementService, UnitOfMeasurementService>();
builder.Services.AddScoped<IIssuanceService, IssuanceService>();
builder.Services.AddScoped<IFileuploadServices, FileUploadService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IGoodsReturnNoteService, GoodsReturnNoteService>();
builder.Services.AddScoped<IIssuanceReturnService, IssuanceReturnService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IBidEvaluationService, BidEvaluationService>();
builder.Services.AddScoped<IQuotationService, QuotationService>();
builder.Services.AddScoped<ICallForQuotationService, CallForQuotationService>();
builder.Services.AddScoped<IQuotationComparativeService, QuotationComparativeService>();
builder.Services.AddScoped<IDepreciationModelService, DepreciationModelService>();
builder.Services.AddScoped<IFixedAssetService, FixedAssetService>();
builder.Services.AddScoped<ICWIPService, CWIPService>();
builder.Services.AddScoped<IDisposalService, DisposalService>();
builder.Services.AddScoped<IBudgetReappropriationService, BudgetReappropriationService>();
builder.Services.AddScoped<IDepreciationAdjustmentService, DepreciationAdjustmentService>();
builder.Services.AddScoped<IFixedAssetReportService, FixedAssetReportService>();
builder.Services.AddScoped<IFacultyService, FacultyService>();
builder.Services.AddScoped<IAcademicDepartmentService, AcademicDepartmentService>();
builder.Services.AddScoped<IDegreeService, DegreeService>();
builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IQualificationService, QualificationService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IFeeItemService, FeeItemService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<IDomicileService, DomicileService>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IBatchService, BatchService>();
builder.Services.AddScoped<IAdmissionCriteriaService, AdmissionCriteriaService>();
builder.Services.AddScoped<IAdmissionApplicationService, AdmissionApplicationService>();
builder.Services.AddScoped<IProgramChallanTemplateService, ProgramChallanTemplateService>();
builder.Services.AddSingleton<IEmailSenderService, EmailSenderService>();
builder.Services.AddHostedService<DepreciationBackgroundService>();
builder.Services.AddScoped<IJournalService, JournalService>();


//Add auto mapper config
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//add HttpContextAccessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilters>()).ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var result = new ValidationFailedResult(context.HttpContext, context.ModelState);
        result.ContentTypes.Add(MediaTypeNames.Application.Json);
        return result;
    };
}).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//For global authorize filter
builder.Services.AddMvc(options =>
{
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Default",
        b =>
        {
            b.WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            builder.Configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .ToArray())
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });

    options.AddPolicy("PayrollModule",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .WithMethods("POST", "GET");
        });
});

var app = builder.Build();

//No need in saas
//UserSeeding.Initialize(app.Services, builder.Configuration["UserCredentials:Password"]);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Default");
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization(); 

app.MapControllers();

app.Run();
