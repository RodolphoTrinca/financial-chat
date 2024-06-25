using FinancialChat.API.Controllers;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddHttpLogging(logging => {
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Financial Chat API",
        Version = "v1",
        Description = "Financial Chat API",
    });
});

builder.Services.AddCors();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseSwagger(options =>
{
    options.RouteTemplate = "api/docs/{documentname}/swagger.json";

    options.PreSerializeFilters.Add((swagger, httpReq) =>
    {
        //Clear servers -element in swagger.json because it got the wrong port when hosted behind reverse proxy
        swagger.Servers.Clear();

    });
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("docs/v1/swagger.json", "Financial Chat API v1");
    c.RoutePrefix = "api";
});

app.MapHub<ChatHub>("/api/chatHub");


app.UseCors(options =>
{
  options.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .SetIsOriginAllowed(origin => true);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
