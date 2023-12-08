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
        Action<NpgsqlDbContextOptionsBuilder> npgSqlOptionsAction = null)
    {
        builder.UseNpgsql(
            connectionString: configuration.GetConnectionString(dbName), 
            npgsqlOptionsAction: npgSqlOptionsAction);

#if DEBUG
        builder.EnableSensitiveDataLogging();
#endif

        return builder;
    }
}