using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using RoosterPlanner.Api.Models.Constants;
using RoosterPlanner.Service;

namespace RoosterPlanner.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #if DEBUG
            IdentityModelEventSource.ShowPII = true; // temp for more logging
            #endif

            // Enable Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.Authority =
                        $"{Configuration["AzureAuthentication:Instance"]}/tfp/{Configuration["AzureAuthentication:AzureTenantId"]}/{Configuration["AzureAuthentication:SignUpSignInPolicyId"]}/v2.0/";
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true
                    };
                    jwtOptions.Audience = Configuration["AzureAuthentication:ClientId"];
                    jwtOptions.Events = new JwtBearerEvents();
                });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigins", builder =>
                {
                    builder.WithOrigins(Configuration.GetSection("AllowedHosts").Value)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddAuthorization(options => { 
                options.AddPolicy("Boardmember", policy =>
                    policy.RequireClaim("extension_UserRole", "1")); //UserRole.Boardmember

                options.AddPolicy("Committeemember", policy =>
                    policy.RequireClaim("extension_UserRole", "2"));

                options.AddPolicy("Boardmember&Committeemember", policy =>
                    policy.RequireClaim("extension_UserRole", "1", "2"));
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
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

            services.AddLogging();

            ServiceContainer.Register(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            app.UseCors("AllowOrigins");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
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