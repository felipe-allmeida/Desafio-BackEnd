using BikeRental.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddOpenApi();
builder.AddApplicationServices();
builder.AddApplicationIntegrationServices();
builder.AddEventBus();

var app = builder.Build();

app.ConfigurePipeline();

app.Run();

public partial class Program { }
