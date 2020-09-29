using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RoosterPlanner.Api.AutoMapperProfiles;
using RoosterPlanner.Common;
using RoosterPlanner.Common.Config;
using RoosterPlanner.Service;

namespace RoosterPlanner.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true; // temp for more logging
            services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.Authority =
                        $"https://login.microsoftonline.com/tfp/{Configuration["AzureAuthentication: TenantId"]}/{Configuration["AzureAuthentication:SignUpSignInPolicyId"]}/";
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidAudiences = new List<string>
                        {
                            Configuration["TokenValidation:ClientIdWeb"],
                            Configuration["TokenValidation:ClientIdHook"]
                        }
                    };
                    jwtOptions.Audience = "c832c923-37c6-4145-8c75-a023ecc7a98f";
                    jwtOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = AuthenticationFailedAsync
                    };
                });
            services.AddCors();
            services.AddAuthorization();

            // Enable Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry();

            services.AddRazorPages().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });


            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.Configure<AzureAuthenticationConfig>(
                Configuration.GetSection(AzureAuthenticationConfig.ConfigSectionName));

            services.AddSingleton<ILogger, Logger>(l =>
            {
                return Logger.Create(Configuration["ApplicationInsight:InstrumentationKey"]);
            });

            services.AddScoped<IAzureB2CService, AzureB2CService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IParticipationService, ParticipationService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IShiftService, ShiftService>();
            services.AddScoped<IMatchService, MatchService>();

            ServiceContainer.Register(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            app.UseCors(i =>
                i.WithOrigins(Configuration.GetSection("AllowedHosts").Value).AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private Task AuthenticationFailedAsync(AuthenticationFailedContext arg)
        {
            // For debugging purposes only!
            var s = $"AuthenticationFailed: {arg.Exception.Message}";
            arg.Response.ContentLength = s.Length;
            arg.Response.Body.Write(Encoding.UTF8.GetBytes(s), 0, s.Length);

            return Task.FromResult(0);
        }
    }
}