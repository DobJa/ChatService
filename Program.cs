using ChatService;
using ChatService.Broker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMessageConsumer, RabbitMQConsumer>();
builder.Services.AddSingleton<IMessageProducer, RabbitMQProducer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.EnableBrokerListener();

app.Run();

public class Message {
    public int UserId {get; set;}
    public int MessId {get; set;}
    public string Text {get; set;}
}
