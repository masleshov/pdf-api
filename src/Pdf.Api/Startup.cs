using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pdf.Api.Configuration;
using Pdf.Api.External;
using Pdf.Api.Infrastructure;
using Pdf.Api.Infrastructure.Constant;
using Pdf.Api.Service;

namespace Pdf.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services
                .AddApiVersioning(setup =>
                {
                    setup.ApiVersionReader = new UrlSegmentApiVersionReader();
                    setup.AssumeDefaultVersionWhenUnspecified = true;
                    setup.ReportApiVersions = true;
                    setup.DefaultApiVersion = Microsoft.AspNetCore.Mvc.ApiVersion.Parse(ApiVersion.Current);
                })
                .AddVersionedApiExplorer(setup =>
                {
                    setup.GroupNameFormat = "'v'VVV";
                    setup.SubstituteApiVersionInUrl = true;
                })
                .AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc($"v{ApiVersion.Current}", new OpenApiInfo { Title = "Pdf API", Version = $"v{ApiVersion.Current}" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }, new string[] { }
                }});

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, null);
            services.AddAuthorization();

            services.AddScoped(sp => new LoggingHttpMessageHandler(sp.GetRequiredService<ILogger<LoggingHttpMessageHandler>>()));

            services.Configure<NpgsqlDatabaseConfiguration>(Configuration.GetSection("NpgsqlDatabase"));
            services.Configure<EmailConfirmationConfiguration>(Configuration.GetSection("EmailConfirmation"));
            services.Configure<SmtpConfiguration>(Configuration.GetSection("Smtp"));
            
            var pdfApiSection = Configuration.GetSection("PdfApi");
            var pdfApiConfiguration = pdfApiSection.Get<PdfApiConfiguration>();

            services.Configure<PdfApiConfiguration>(pdfApiSection);

            services.AddTransient<PdfApiHttpHandler>();
            services.AddRefitClient<IPdfApiExternalClient>(new Uri(pdfApiConfiguration.Uri))
                .AddHttpMessageHandler<PdfApiHttpHandler>();

            services.AddTransient<UnitOfWork>();
            services.AddSingleton<JobStatusChecker>();

            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IAuthorizationInfoService, AuthorizationInfoService>();
            services.AddTransient<IEmailConfirmationService, EmailConfirmationService>();

            services.AddTransient<IEmailToPdfConvertService, EmailToPdfConvertService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pdf API"));

            app.UseApiVersioning();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseExceptionHandler(builder => builder.UseResponseExceptionHandler(env));
            app.UseWebSockets();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
