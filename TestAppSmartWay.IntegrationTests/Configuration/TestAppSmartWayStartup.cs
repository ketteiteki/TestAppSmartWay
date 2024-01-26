using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Microsoft.Extensions.DependencyInjection;
using TestAppSmartWay.Application.BusinessLogic;
using TestAppSmartWay.Infrastructure.Migrations;
using TestAppSmartWay.Infrastructure.Repositories;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;

namespace TestAppSmartWay.IntegrationTests.Configuration;

public class TestAppSmartWayStartup
{
    public static IServiceProvider Initialize(string connectionString)
    {
        var serviceCollection = new ServiceCollection();
        
        serviceCollection
            .AddFluentMigratorCore()
            .ConfigureRunner(rb =>
            {
                rb.AddPostgres();
                rb.WithGlobalConnectionString(connectionString);
                rb.ScanIn(typeof(Tables).Assembly).For.Migrations();
            });

        serviceCollection.AddSingleton(connectionString);
        
        // repositories
        serviceCollection.AddSingleton<ICompanyRepository, CompanyRepository>();
        serviceCollection.AddSingleton<IDepartmentRepository, DepartmentRepository>();
        serviceCollection.AddSingleton<IEmployeeRepository, EmployeeRepository>();
        serviceCollection.AddSingleton<IPassportRepository, PassportRepository>();

        // services
        serviceCollection.AddSingleton<EmployeeService>();
        
        serviceCollection.AddScoped<IConventionSet>(_ => new DefaultConventionSet("public", ""));
        
        return serviceCollection.BuildServiceProvider();
    }
}