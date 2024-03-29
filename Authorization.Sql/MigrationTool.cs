﻿using System;
using System.Collections.Generic;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Authorization.Sql;

public sealed class MigrationTool
{
    private readonly IServiceProvider _rootServiceProvider;
    private readonly ILogger<MigrationTool> _logger;

    public MigrationTool(IServiceProvider rootServiceProvider)
    {
        _rootServiceProvider = rootServiceProvider;
        _logger = rootServiceProvider.GetRequiredService<ILogger<MigrationTool>>();
    }

    public static void Execute(IServiceProvider serviceProvider) 
        => new MigrationTool(serviceProvider).Migrate();

    private void Migrate()
    {
        _logger.LogInformation("Creating scope...");

        try
        {
            using var scope = _rootServiceProvider.CreateScope();

            var dbContextCollection = ResolveDbContextCollection(scope.ServiceProvider);
        
            foreach (var dbContext in dbContextCollection)
            {
                _logger.LogInformation("Migrating DbContext '{DbContext}'...", dbContext.GetType());
#if DEBUG
                _logger.LogInformation("ConnectionString: {ConnectionString}", dbContext.Database.GetConnectionString());
#endif
                dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(2));
                dbContext.Database.Migrate();
                dbContext.Database.SetCommandTimeout(TimeSpan.FromSeconds(30));

                _logger.LogInformation("Migrate for DbContext '{DbContext}' is complete", dbContext.GetType());
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while applying migration");
            throw;
        }

        _logger.LogInformation("Migrations are complete");
    }

    private static IEnumerable<DbContext> ResolveDbContextCollection(IServiceProvider serviceProvider)
    {
        yield return serviceProvider.GetRequiredService<AuthorizationDbContext>();
        yield return serviceProvider.GetRequiredService<PersistedGrantDbContext>();
    }
}