using Asp.Versioning;
using EFCoreSecondLevelCacheInterceptor;
using Ems.Domain.Jobs;
using Ems.Domain.Services;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storage;
using Ems.Interceptors;
using Ems.Models;
using Ems.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl.AdoJobStore;
using System.Text;
using System.Text.Json;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
});
builder.Services.AddApiVersioning(opt =>
{
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
    opt.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddSingleton<AuthenticationFilter>();
builder.Services.AddScoped<CurrentClassBindingActionFilter>();
builder.Services.AddScoped(typeof(ValidationActionFilter<>));
builder.Services.Configure<ApiBehaviorOptions>(opt => { opt.SuppressInferBindingSourcesForParameters = true; });
builder.Services.AddControllers(x =>
{
    if (builder.Environment.IsDevelopment())
        x.Filters.Add<RequestTimeStampActionFilter>();
    x.Filters.AddService<CurrentClassBindingActionFilter>();
    x.Filters.AddService<AuthenticationFilter>();
}).AddJsonOptions(
    opt =>
    {
        if (builder.Environment.IsDevelopment()) opt.JsonSerializerOptions.WriteIndented = true;

        opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opt.AllowInputFormatterExceptionMessages = true;
    });
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Version 1", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Bearer token",
        Name = HeaderNames.Authorization,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    opt.OperationFilter<SwaggerODataFilter>();
    opt.OperationFilter<SwaggerJsonIgnoreFilter>();
});

builder.Services.AddOptions();
builder.Services.AddAuthorization();
builder.Services.AddValidatorsFromAssembly(typeof(Ems.Models.AssemblyMarker).Assembly);

builder.Services.AddHttpContextAccessor();

builder.Services.AddHostedService<DbInitializer>();
builder.Services.AddQuartzHostedService();
builder.Services.AddQuartz(opt =>
{
    var quartzConnection = builder.Configuration.GetConnectionString("Database");
    opt.UseMicrosoftDependencyInjectionJobFactory();
    opt.UsePersistentStore(storeOpt =>
    {
        storeOpt.UsePostgres(postgresOpts =>
        {
            postgresOpts.UseDriverDelegate<PostgreSQLDelegate>();
            postgresOpts.ConnectionString = quartzConnection!;
        });
        storeOpt.UseJsonSerializer();
    });
});
builder.Services.Configure<StudentRecordOptions>(builder.Configuration.GetSection(nameof(StudentRecordOptions)).Bind);
builder.Services.Configure<ClassVersionOptions>(builder.Configuration.GetSection(nameof(ClassVersionOptions)).Bind);
builder.Services.Configure<QuarterSlideJob>(builder.Configuration.GetSection(nameof(QuarterSlideJob)).Bind);
builder.Services.Configure<QrCodeOptions>(builder.Configuration.GetSection(nameof(QrCodeOptions)).Bind);
builder.Services.Configure<GeolocationStudentRecordSessionOptions>(builder.Configuration
    .GetSection(nameof(GeolocationStudentRecordSessionOptions)).Bind);
builder.Services.Configure<QrCodeStudentRecordSessionOptions>(builder.Configuration
    .GetSection(nameof(QrCodeStudentRecordSessionOptions)).Bind);

builder.Services.AddMemoryCache();
builder.Services.AddEFSecondLevelCache(opt =>
{
    opt.CacheAllQueries(CacheExpirationMode.Sliding, TimeSpan.FromMinutes(1));
    opt.UseMemoryCacheProvider();
});
builder.Services.AddDbContext<EmsDbContext>((provider, opt) =>
{
    opt.AddInterceptors(new EntityInterceptor(), provider.GetRequiredService<SecondLevelCacheInterceptor>());
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    var dbConnection = builder.Configuration.GetConnectionString("Database");
    opt.UseNpgsql(dbConnection,
        providerOpts =>
        {
            providerOpts.MigrationsAssembly(typeof(Ems.Infrastructure.AssemblyMarker).Assembly.FullName);
        });
});
builder.Services.AddScoped<IClassPeriodService, ClassPeriodService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<IClassVersionService, ClassVersionService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ILecturerService, LecturerService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IIdlePeriodService, IdlePeriodService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IStudentRecordSessionService, StudentRecordSessionService>();
builder.Services.AddScoped<IStudentRecordService, StudentRecordService>();

builder.Services.AddSingleton<ResponseTimeMiddleware>();

builder.Services.AddScoped<IScheduleService<PublishClassVersionJob>, PublishClassVersionJob.ScheduleService>();
builder.Services.AddScoped<IScheduleService<QuarterSlideJob>, QuarterSlideJob.ScheduleService>();
builder.Services.AddScoped<IScheduleService<GeolocationStudentRecordSessionJob>, GeolocationStudentRecordSessionJob.ScheduleService>();

builder.Services.AddSingleton<IQrCodeGenerator, QrCodeGenerator>();
builder.Services.AddAutoMapper(typeof(Ems.Domain.AssemblyMarker).Assembly);

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(corsBuilder => { corsBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
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

    app.UseMiddleware<ResponseTimeMiddleware>();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();