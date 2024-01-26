using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using TestAppSmartWay.Application.BusinessLogic;
using TestAppSmartWay.Domain.Constants;
using TestAppSmartWay.Infrastructure.Migrations;
using TestAppSmartWay.Infrastructure.Repositories;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;
using TestAppSmartWay.WebApi.Extensions;
using TestAppSmartWay.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString(AppSettingsConstants.SqlDatabaseConnection);
var dbmsConnectionString = builder.Configuration.GetConnectionString(AppSettingsConstants.SqlDbmsConnection);

ArgumentException.ThrowIfNullOrEmpty(connectionString);
ArgumentException.ThrowIfNullOrEmpty(dbmsConnectionString);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddSingleton(connectionString);

// repositories
builder.Services.AddSingleton<ICompanyRepository, CompanyRepository>();
builder.Services.AddSingleton<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddSingleton<IPassportRepository, PassportRepository>();

// services
builder.Services.AddSingleton<EmployeeService>();

builder.Services.AddScoped<IConventionSet>(_ => new DefaultConventionSet("public", ""));

builder.Services
    .AddFluentMigratorCore()
    .ConfigureRunner(rb =>
    {
        rb.AddPostgres();
        rb.WithGlobalConnectionString(connectionString);
        rb.ScanIn(typeof(Tables).Assembly).For.Migrations();
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<ValidationMiddleware>();

app.MapControllers();

await app.Services.MigrateDatabase(dbmsConnectionString, connectionString);

app.Run();
