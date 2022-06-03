using System.Reflection;
using ChatService.Models;
using ChatService.Services;
using MassTransit;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables(prefix: "HPDS_COMMON_");

// Add services to the container.
builder.Services.Configure<ChatDatabaseSettings>(
    builder.Configuration.GetSection("ChatDatabase"));

builder.Services.AddSingleton<MessagesService>();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins(builder.Configuration["HPDS_COMMON_FRONTEND_URL"])
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    var entryAssembly = Assembly.GetEntryAssembly();

    x.AddConsumers(entryAssembly);

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["HPDS_COMMON_RABBITMQ_HOST"], builder.Configuration["HPDS_COMMON_RABBITMQ_VHOST"], h => { 
            h.Username(builder.Configuration["HPDS_COMMON_RABBITMQ_USERNAME"]);
            h.Password(builder.Configuration["HPDS_COMMON_RABBITMQ_PASSWORD"]);
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors();

app.MapGet("/messages", async (MessagesService messagesService) => await messagesService.GetAsync());

app.Run();