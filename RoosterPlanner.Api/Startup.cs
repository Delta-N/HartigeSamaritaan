using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Identity.Web;
using RoosterPlanner.Api.AutoMapperProfiles;
using RoosterPlanner.Common;
using RoosterPlanner.Common.Config;
using RoosterPlanner.Service;

namespace RoosterPlanner.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        readonly string AllowOrigins = "_allowOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true; // temp for more logging

            services.AddMicrosoftIdentityWebApiAuthentication(Configuration, "AzureAd");
            
            /*services.AddAuthentication(options => {
             options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
             })
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
                });*/

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigins", builder =>
                {
                    builder.WithOrigins(Configuration.GetSection("AllowedHosts").Value)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddAuthorization();

            // Enable Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry();
            
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = false;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            services.Configure<AzureAuthenticationConfig>(
                Configuration.GetSection(AzureAuthenticationConfig.ConfigSectionName));
            
            services.AddTransient<IAzureB2CService, AzureB2CService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IParticipationService, ParticipationService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IShiftService, ShiftService>();
            services.AddTransient<IMatchService, MatchService>();

            //dit moet nog omgebouwd worden
            services.AddAutoMapper(typeof(AutoMapperProfile));
            
            services.AddSingleton<ILogger, Logger>(l =>
            {
                return Logger.Create(Configuration["ApplicationInsight:InstrumentationKey"]);
            });

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

            app.UseCors(AllowOrigins);
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
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