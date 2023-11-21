using Authorization.Validation.Authorization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Globalization;
using System.IO;
using Authorization.Configuration;
using Authorization.Core;

namespace Authorization;

public class Startup
{
    private const string ApiName = "Authorization";
    private readonly Version _assemblyVersion;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        _assemblyVersion = new Version(1, 0);
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAllDbContext(Configuration);

        services.AddRouting(c => c.LowercaseUrls = true);

        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetailsFactory =
                        context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                    var problemDetails = problemDetailsFactory
                        .CreateValidationProblemDetails(context.HttpContext, context.ModelState, statusCode: 400);
                    problemDetails.Title = "Произошла ошибка валидации!";
                    var result = new BadRequestObjectResult(problemDetails);

                    return result;
                };
            })
            .AddNewtonsoftJson(cfg =>
            {
                cfg.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                cfg.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            });

        services.AddAutoMapper(x => x.AddMaps(typeof(MappingProfile).Assembly));

        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(type => type.ToString());
            c.CustomOperationIds(d => (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName);
            c.SwaggerDoc($"v{_assemblyVersion}", new OpenApiInfo
            {
                Version = $"v{_assemblyVersion}",
                Title = $"{ApiName} API",
            });

            var xmlContractDocs = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory), "*.xml");
            foreach (var fileName in xmlContractDocs) c.IncludeXmlComments(fileName);
        });

        services.AddMvc(opt => { opt.EnableEndpointRouting = false; })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<UserParametersValidator>();

                fv.ValidatorOptions.LanguageManager.Enabled = true;
                fv.ValidatorOptions.LanguageManager.Culture = new CultureInfo("ru-RU");
            });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.ConfigureOptions(Configuration)
            .AddAuth()
            .AddServices()
            .AddRepositories()
            .AddMemoryCache();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseCors(options =>
        {
            options.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });

        app.UseStatusCodePages();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseSwagger(c => { c.SerializeAsV2 = true; });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"/swagger/v{_assemblyVersion}/swagger.json", $"{ApiName} API V{_assemblyVersion}");
            c.RoutePrefix = string.Empty;
            c.DocumentTitle = $"{ApiName} Documentation";
            c.DocExpansion(DocExpansion.None);
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}