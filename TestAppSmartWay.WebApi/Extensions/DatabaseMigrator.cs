using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace TestAppSmartWay.WebApi.Extensions;

public static class DatabaseMigrator
{
    public static async Task MigrateDatabase(this IServiceProvider serviceProvider, string dbmsConnectionString, string databaseConnectionString)
    {
        var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(databaseConnectionString);

        if (!sqlConnectionStringBuilder.TryGetValue("Database", out var databaseName))
        {
            throw new Exception("Incorrect database connection string");
        } 
        
        var connection = new NpgsqlConnection(dbmsConnectionString);
        var isThereDatabase = await connection.ExecuteScalarAsync<bool>($"""select exists(select * from pg_database where datname = '{databaseName}');""");

        if (isThereDatabase) return;
        
        await connection.ExecuteAsync($"""create database "{databaseName}";""");

        var provider = serviceProvider.CreateScope().ServiceProvider;
        var runner = provider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}