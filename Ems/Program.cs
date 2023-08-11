using System.Text.Json;
using Ems.Constants;
using Ems.Domain.Jobs;
using Ems.Domain.Services;
using Ems.Domain.Services.Import;
using Ems.Domain.Services.Scheduling;
using Ems.Infrastructure.Exceptions;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storages;
using Ems.Interceptors;
using Ems.Models;
using Ems.Services;
using Ems.Services.Hooks;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

var startupHooks = new IStartupHook[] { new ExcelStartupHook() };
foreach (var startupHook in startupHooks) await startupHook.Execute();

builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
});
builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'V";
    opt.SubstituteApiVersionInUrl = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddScoped(typeof(ValidationActionFilter<>));
builder.Services.Configure<ApiBehaviorOptions>(opt => { opt.SuppressInferBindingSourcesForParameters = true; });
builder.Services.AddControllers().AddJsonOptions(
    opt =>
    {
        if (builder.Environment.IsDevelopment()) opt.JsonSerializerOptions.WriteIndented = true;

        opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opt.AllowInputFormatterExceptionMessages = true;
    });
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Version 1", Version = "v1" });
});

builder.Services.AddOptions();
builder.Services.AddAuthentication().AddJwtBearer(opt =>
{
    var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
    if (jwtOptions is null) throw new StartupException($"{nameof(JwtOptions)} is not found in configuration");
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256, SecurityAlgorithms.Aes128CbcHmacSha256 },
        ValidAudience = jwtOptions.Audience,
        ValidIssuer = jwtOptions.Issuer,
        IssuerSigningKey = jwtOptions.SigningSecurityKey,
        TokenDecryptionKey = jwtOptions.EncryptingSecurityKey
    };
    if (builder.Environment.IsDevelopment()) opt.IncludeErrorDetails = true;
});
builder.Services.AddAuthorization();
builder.Services.AddValidatorsFromAssembly(Assemblies.Domain);
builder.Services.AddScoped<IPasswordProvider, PasswordProvider>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHostedService<DbInitializer>();
builder.Services.AddHostedService<EmailBackgroundService>();
builder.Services.AddHostedService<AdminAccountInitializer>();

builder.Services.AddQuartzHostedService();
builder.Services.AddQuartz(opt =>
{
    var quartzConnection = builder.Configuration.GetConnectionString(Connections.Quartz);
    opt.UseMicrosoftDependencyInjectionJobFactory();
    opt.UsePersistentStore(storeOpt =>
    {
        storeOpt.UsePostgres(quartzConnection!);
        storeOpt.UseJsonSerializer();
    });
});
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.Configure<AccountOptions>(builder.Configuration.GetSection(nameof(AccountOptions)).Bind);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)).Bind);
builder.Services.Configure<StudentRecordOptions>(builder.Configuration.GetSection(nameof(StudentRecordOptions)).Bind);
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(nameof(EmailOptions)).Bind);
builder.Services.Configure<ClassVersionOptions>(builder.Configuration.GetSection(nameof(ClassVersionOptions)).Bind);
builder.Services.Configure<QuarterSlideJob>(builder.Configuration.GetSection(nameof(QuarterSlideJob)).Bind);
builder.Services.Configure<EmailSenderOptions>(builder.Configuration.GetSection(nameof(EmailSenderOptions)).Bind);
builder.Services.Configure<AdminAccountOptions>(builder.Configuration.GetSection(nameof(AdminAccountOptions)).Bind);

builder.Services.AddDbContext<EmsDbContext>(opt =>
{
    opt.AddInterceptors(new EntityInterceptor());
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    var dbConnection = builder.Configuration.GetConnectionString(Connections.Db);
    opt.UseNpgsql(dbConnection, npgsqlOpt => { npgsqlOpt.MigrationsAssembly(Assemblies.Infrastructure); });
});
builder.Services.AddScoped<ImportServiceProvider>();
builder.Services.AddScoped<ExcelClassPeriodImportService>();
builder.Services.AddScoped<ExcelClassroomImportService>();
builder.Services.AddScoped<ExcelClassVersionImportService>();
builder.Services.AddScoped<ExcelGroupImportService>();
builder.Services.AddScoped<ExcelLecturerImportService>();
builder.Services.AddScoped<ExcelLessonImportService>();
builder.Services.AddScoped<ExcelStudentImportService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IClassPeriodService, ClassPeriodService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<IClassVersionService, ClassVersionService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ILecturerService, LecturerService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IIdlePeriodService, IdlePeriodService>();
builder.Services.AddScoped<IExternalAccountService, ExternalAccountService>();

builder.Services.AddScoped<IScheduleService<PublishClassVersionJob>, PublishClassVersionJobQuartzScheduleService>();
builder.Services.AddScoped<IScheduleService<QuarterSlideJob>, QuarterSlideJobQuartzScheduleService>();

builder.Services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddSingleton<IUrlService, UrlService>();
builder.Services.AddAutoMapper(Assemblies.Domain);

builder.Services.AddCors(opt =>
{
    opt.DefaultPolicyName = CorsPolicies.PublicCors;
    opt.AddPolicy(CorsPolicies.PublicCors,
        corsBuilder => { corsBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(error =>
    {
        error.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature is not null)
                await context.Response.WriteAsJsonAsync(new ExceptionResult
                {
                    Message = exceptionFeature.Error.Message
                });
        });
    });
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();