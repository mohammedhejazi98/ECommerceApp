using InventoryService.BackgroundServices;
using InventoryService.Common.Constants.RabbitMq;
using InventoryService.Common.Contracts.Infrastructure;
using InventoryService.Data;
using InventoryService.EventProcessing;
using InventoryService.Services.ServiceBus;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMessagesServiceBus, MessagesServiceBus>();
builder.Services.AddScoped<ISendToRabbitMqConsumer, SendToRabbitMqConsumer>();
builder.Services.AddSingleton<IOrderEventProcessor, OrderEventProcessor>();

RabbitMqConnection.Configure(builder.Configuration);
builder.Services.AddHostedService<OrderMessageSubscriber>();

#region Register our context

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

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
