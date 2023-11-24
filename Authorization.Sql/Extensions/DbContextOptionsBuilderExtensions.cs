using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System;

namespace Authorization.Sql.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder DefaultDataBaseConfiguration(
        this DbContextOptionsBuilder builder, 
        IConfiguration configuration,
        string dbName,
        Action<NpgsqlDbContextOptionsBuilder> npgsqlOptionsAction = null)
    {
        builder.UseNpgsql(
            connectionString: configuration.GetConnectionString(dbName), 
            npgsqlOptionsAction: npgsqlOptionsAction);

        builder.EnableSensitiveDataLogging();

        return builder;
    }
}