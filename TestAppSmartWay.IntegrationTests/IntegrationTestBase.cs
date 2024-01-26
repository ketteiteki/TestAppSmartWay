using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using TestAppSmartWay.Application.BusinessLogic;
using TestAppSmartWay.Domain.Constants;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;
using TestAppSmartWay.IntegrationTests.Configuration;
using TestAppSmartWay.WebApi.Extensions;
using Xunit;

namespace TestAppSmartWay.IntegrationTests;

[Collection("Sequence")]
public class IntegrationTestBase : IAsyncLifetime
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    protected readonly ICompanyRepository CompanyRepository;
    protected readonly IDepartmentRepository DepartmentRepository;
    protected readonly IEmployeeRepository EmployeeRepository;
    protected readonly IPassportRepository PassportRepository;
    protected readonly EmployeeService EmployeeService;

    public IntegrationTestBase()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var databaseConnectionString = _configuration.GetConnectionString(AppSettingsConstants.SqlIntegrationTestsDatabaseConnection);
        
        ArgumentException.ThrowIfNullOrEmpty(databaseConnectionString);
        
        _serviceProvider = TestAppSmartWayStartup.Initialize(databaseConnectionString);

        // repositories
        CompanyRepository = _serviceProvider.GetRequiredService<ICompanyRepository>();
        DepartmentRepository = _serviceProvider.GetRequiredService<IDepartmentRepository>();
        EmployeeRepository = _serviceProvider.GetRequiredService<IEmployeeRepository>();
        PassportRepository = _serviceProvider.GetRequiredService<IPassportRepository>();
        EmployeeService = _serviceProvider.GetRequiredService<EmployeeService>();
    }
    
    public async Task InitializeAsync()
    {
        var databaseConnectionString = _configuration.GetConnectionString(AppSettingsConstants.SqlIntegrationTestsDatabaseConnection);
        var dbmsConnectionString = _configuration.GetConnectionString(AppSettingsConstants.SqlDbmsConnection);
        
        ArgumentException.ThrowIfNullOrEmpty(databaseConnectionString);
        ArgumentException.ThrowIfNullOrEmpty(dbmsConnectionString);
        
        await _serviceProvider.MigrateDatabase(dbmsConnectionString, databaseConnectionString);

        await using var connection = new NpgsqlConnection(databaseConnectionString); 
        
        var query = """
                    truncate table "EmployeeEntity" cascade;
                    truncate table "CompanyEntity" cascade;
                    truncate table "PassportEntity" cascade;
                    truncate table "DepartmentEntity" cascade;
                    """;

        await connection.ExecuteAsync(query);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    
    public NpgsqlConnection GetConnection()
    {
        var databaseConnectionString = _configuration.GetConnectionString(AppSettingsConstants.SqlIntegrationTestsDatabaseConnection);
        
        return new NpgsqlConnection(databaseConnectionString);
    }
}