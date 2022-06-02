using System.Reflection;
using ChatService.Models;
using ChatService.Services;
using MassTransit;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ChatDatabaseSettings>(
    builder.Configuration.GetSection("ChatDatabase"));

builder.Services.AddSingleton<MessagesService>();

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
        cfg.Host("rabbitmq", "/", h => { 
            h.Username("guest");
            h.Password("guest");
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

app.MapGet("/messages", async (MessagesService messagesService) => await messagesService.GetAsync());

app.Run();