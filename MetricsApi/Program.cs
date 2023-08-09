using MetricsApi.Domain.Repository;
using MetricsApi.Domain.Storage;
using MetricsApi.Hubs;
using MetricsApi.Repository;
using MetricsApi.Service;
using MetricsApi.Service.Metrics;
using MetricsApi.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddSingleton<IConnectorCreator, PostgresConnectorCreator>();
builder.Services.AddHostedService<UpdateDbService>();
builder.Services.AddScoped<IMetricsRepository, MetricsRepository>();
builder.Services.AddScoped<IMetricsService, MetricsService>();
builder.Services.AddSingleton<PeriodService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapHub<MetricsHub>("metrics-hub");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();


