using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database connection

builder.Services.AddDbContext<DataContext>(
    options => options.UseLazyLoadingProxies()
                      .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Hosted services
builder.Services.AddHostedService<UpdateCurrency>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();