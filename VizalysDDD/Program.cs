
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
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();

//Add auto mapper config
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
UserSeeding.Initialize(app.Services);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
