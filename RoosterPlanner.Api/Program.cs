using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using CorrelationId.DependencyInjection;
using Microsoft.Identity.Web;
using RoosterPlanner.Api.Models.Constants;
using RoosterPlanner.Service.Services;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Service.Config;
using System.Net.Mail;
using RoosterPlanner.Email;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

//Add Azure logging
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", p =>
    {
        p.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    }
    );
});


builder.Services.AddDefaultCorrelationId();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind("AzureAuthentication", options);
    },
    options =>
    {
        builder.Configuration.Bind("AzureAuthentication", options);
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Boardmember", policy => policy.RequireClaim("extension_UserRole", "1"))
    .AddPolicy("Committeemember", policy => policy.RequireClaim("extension_UserRole", "2"))
    .AddPolicy("Boardmember&Committeemember", policy => policy.RequireClaim("extension_UserRole", "1", "2"));

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
        new OpenApiSecurityScheme {
        Reference = new OpenApiReference {
        Type = ReferenceType.SecurityScheme,
        Id = "Bearer"
        }
        },
        Array.Empty<string>()
        }
        });
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "RoosterPlanner", Version = "v1" });


    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("RoosterPlannerDatabase")!, tags: new[] { "database" });

builder.Services.AddLogging();

builder.Services.Configure<WebUrlConfig>(
builder.Configuration.GetSection(WebUrlConfig.ConfigSectionName));

builder.Services.AddSingleton(builder.Configuration.GetValue<string>("AzureAuthentication:B2CExtentionApplicationId"));
builder.Services.AddTransient<IAzureB2CService, AzureB2CService>();
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddTransient<IParticipationService, ParticipationService>();
builder.Services.AddTransient<ITaskService, TaskService>();
builder.Services.AddTransient<IShiftService, ShiftService>();
builder.Services.AddTransient<IBlobService, BlobService>();
builder.Services.AddTransient<IAvailabilityService, AvailabilityService>();
builder.Services.AddTransient<IDocumentService, DocumentService>();
builder.Services.AddTransient<ICertificateService, CertificateService>();
builder.Services.AddTransient<IRequirementService, RequirementService>();


builder.Services.AddDbContext<RoosterPlannerContext>(options =>
{
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("RoosterPlannerDatabase"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.Configure<EmailConfig>(options =>
    builder.Configuration.GetSection(EmailConfig.ConfigSectionName).Bind(options));

builder.Services.Configure<AzureBlobConfig>(options =>
    builder.Configuration.GetSection(AzureBlobConfig.ConfigSectionName).Bind(options));

builder.Services.Configure<AzureAuthenticationConfig>(options =>
    builder.Configuration.GetSection(AzureAuthenticationConfig.ConfigSectionName).Bind(options));

builder.Services.AddScoped<IEmailService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    EmailConfig config = new();
    configuration.Bind(EmailConfig.ConfigSectionName, config);
    SmtpClient smtpClient = new(config.SMTPadres)
    {
        Port = config.Port,
        Credentials = new NetworkCredential(config.Emailadres, config.Password),
        EnableSsl = config.EnableSsl
    };

    return new SMTPEmailService(smtpClient, config.Emailadres);
});

var app = builder.Build();

// This works perfectly fine for a simple application running on a single instance.
if (!string.IsNullOrEmpty(builder.Configuration.GetConnectionString("RoosterPlannerDatabase")))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<RoosterPlannerContext>();
    db.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.EnableTryItOutByDefault();
        c.DisplayRequestDuration();
        c.EnableFilter();
        c.OAuthAppName("DevHub");
    });
}
else
{
    app.UseHsts();
}

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    AllowCachingResponses = false
});
app.MapHealthChecks("/", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    AllowCachingResponses = true
});

app.Logger.LogInformation("Starting application...");
app.Run();