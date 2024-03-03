using System;
using System.Text.Json.Serialization;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using RoosterPlanner.Api.Middleware;
using RoosterPlanner.Api.Models.Constants;
using RoosterPlanner.Service;
using RoosterPlanner.Service.Services;
using Serilog;

namespace RoosterPlanner.Api {
    public class Startup {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
#if DEBUG
            IdentityModelEventSource.ShowPII = true; // temp for more logging
#endif

            services.AddDefaultCorrelationId();

            // Enable Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry(Configuration);


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options => {
                Configuration.Bind("AzureAuthentication", options);

                // options.TokenValidationParameters.NameClaimType = "name";
                // options.TokenValidationParameters.RoleClaimType = "groups";
            },
            options => { Configuration.Bind("AzureAuthentication", options); });

            // services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
            //     .AddJwtBearer(jwtOptions =>
            //     {
            //         jwtOptions.Authority =
            //             $"{Configuration["AzureAuthentication:Instance"]}/tfp/{Configuration["AzureAuthentication:TenantId"]}/{Configuration["AzureAuthentication:SignUpSignInPolicyId"]}/v2.0/";
            //         jwtOptions.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             ValidateIssuer = true,
            //             ValidateLifetime = true
            //         };
            //         jwtOptions.Audience = Configuration["AzureAuthentication:ClientId"];
            //         jwtOptions.Events = new JwtBearerEvents();
            //     });

            services.AddCors(options => {
                options.AddPolicy("AllowSpecificOrigins", p => {
                    p.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                }
                );
            });

            services.AddAuthorization(options => {
                options.AddPolicy("Boardmember", policy =>
                policy.RequireClaim("extension_UserRole", "1")); //UserRole.Boardmember

                options.AddPolicy("Committeemember", policy =>
                policy.RequireClaim("extension_UserRole", "2"));

                options.AddPolicy("Boardmember&Committeemember", policy =>
                policy.RequireClaim("extension_UserRole", "1", "2"));
            });


            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
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

            services.AddHealthChecks()
            .AddSqlServer(Configuration.GetConnectionString("RoosterPlannerDatabase")!, tags: new[] { "database" });


            services.AddLogging();

            ServiceContainer.Register(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env) {

            app.UseCorrelationId();
            if (env.IsDevelopment()) {
                IdentityModelEventSource.ShowPII = false;
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options => {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });

            }
            else {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowSpecificOrigins");


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health").AllowAnonymous();
            });
        }
        private void AddCustomServices(IServiceCollection services) {
            services.Configure<WebUrlConfig>(
            Configuration.GetSection(WebUrlConfig.ConfigSectionName));

            services.AddSingleton(
            Configuration.GetValue<string>("AzureAuthentication:B2CExtentionApplicationId"));
            services.AddTransient<IAzureB2CService, AzureB2CService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IParticipationService, ParticipationService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IShiftService, ShiftService>();
            services.AddTransient<IBlobService, BlobService>();
            services.AddTransient<IAvailabilityService, AvailabilityService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<ICertificateService, CertificateService>();
            services.AddTransient<IRequirementService, RequirementService>();

        }

        /*private Task AuthenticationFailedAsync(AuthenticationFailedContext arg)
        {
            // For debugging purposes only!
            var s = $"AuthenticationFailed: {arg.Exception.Message}";
            arg.Response.ContentLength = s.Length;
            arg.Response.Body.Write(Encoding.UTF8.GetBytes(s), 0, s.Length);

            return Task.FromResult(0);
        }*/
    }
}
