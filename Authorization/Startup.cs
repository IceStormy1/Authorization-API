using Authorization.Configuration;
using Authorization.Entities.Entities;
using Authorization.Sql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;

namespace Authorization;

public class Startup
{
    private const string ApiName = "Authorization";
    private readonly Version _assemblyVersion;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        _assemblyVersion = new Version(1, 0);
        IdentityModelEventSource.ShowPII = true;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddIdentity<UserEntity, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();

        services //.AddSameSiteCookiePolicy()
            .AddAntiforgery(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            })
            .ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Auth.IdentityCookie";
                config.LoginPath = "/Account/Login";
                config.LogoutPath = "/Account/Logout";
                config.Cookie.SameSite = SameSiteMode.Lax;
                config.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            })
            .ConfigureOptions(Configuration)
            .AddAllDbContext(Configuration)
            .AddRouting(c => c.LowercaseUrls = true)
            .RegisterIdentityServer(Configuration);

        services.AddControllersWithViews();

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
            .AddSessionStateTempDataProvider();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddAuth()
            .AddHttpContextAccessor()
            .AddSession()
            .AddServices()
            .AddRepositories()
            .AddMemoryCache();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseHttpsRedirection()
            .UseHsts()
            .UseCors(options =>
            {
                options.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
     
        app.Use(async (context, next) =>
        {
            context.Request.EnableBuffering();
            if (context.Request.Headers.ContainsKey("X-Original-Path"))
            {
                context.Request.PathBase = context.Request.Headers["X-Original-Path"].ToString();
            }
            await next();
        });

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        app.UseStaticFiles();

        app.UseStatusCodePages();

        app.UseIdentityServer();

        app.UseCookiePolicy(new CookiePolicyOptions { Secure = CookieSecurePolicy.Always });

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