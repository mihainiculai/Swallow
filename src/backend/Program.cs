using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;
using Swallow.Data;
using Swallow.Extensions;
using Hangfire;
using Microsoft.AspNetCore.Localization;
using Swallow.Exceptions.Handlers;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });

// Database connection

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseLazyLoadingProxies()
           .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddExceptionHandler<GeneralExceptionHandler>();

builder.Services.AddAuthenticationServices();
builder.Services.AddCustomServices();
builder.Services.AddHttpClients();
builder.Services.AddRepositories();

builder.Services.AddHangfire(configuration => configuration.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions { TableName = "ErrorLogs" },
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error
    )
    .CreateLogger();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseExceptionHandler(_ => { });

app.UseHangfireDashboard();

BackgroundJob.Enqueue<IDatabaseInitializer>(x => x.InitializeAsync());
//BackgroundJob.Enqueue<ICurrencyUpdater>(x => x.UpdateCurrenciesAsync());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
    .WithOrigins("https://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
);


var supportedCultures = new[] { "en-US" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();