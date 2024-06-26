using FinancialChat.API.Controllers;
using FinancialChat.Application.Entities.Configuration.RabbitMQ;
using FinancialChat.Application.Interfaces.Gateways;
using FinancialChat.Application.Interfaces.Repositorys;
using FinancialChat.Application.Interfaces.Services;
using FinancialChat.Application.Services;
using FinancialChat.Application.SignalR;
using FinancialChat.Infra.Context;
using FinancialChat.Infra.RabbitMQ.Configuration;
using FinancialChat.Infra.RabbitMQ.Consumers;
using FinancialChat.Infra.RabbitMQ.Producers;
using FinancialChat.Infra.Repository;
using FinancialChat.Worker;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Financial Chat API",
        Version = "v1",
        Description = "Financial Chat API",
    });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

//Adding SignalR 
builder.Services.AddSignalR(options =>
{
    options.DisableImplicitFromServicesParameters = true;
});

//Serilog configuration
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddHttpLogging(logging => {
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddCors();

//Rabbit MQ Configuration
builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQConnectionSettings"));
builder.Services.Configure<RabbitMQQueueNames>(builder.Configuration.GetSection("RabbitMQQueueNames"));
builder.Services.AddSingleton<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();
builder.Services.AddSingleton<IRabbitMQConsumer, HubConnectionMessageConsumer>();
builder.Services.AddScoped<IStockRequestProducer, StockRequestProducer>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();

//Identity configurations
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MessagesDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDbContext<FinancialChatContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<FinancialChatContext>();
builder.Services.AddAuthorization();

//Configuring Services
builder.Services.AddScoped<IChatCommandService, ChatCommandService>();

builder.Services.AddHostedService<ConsumerWorker>();

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

app.MapGroup("/api").MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
