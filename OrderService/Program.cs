using Microsoft.EntityFrameworkCore;

using OrderService.Common.Constants.RabbitMq;
using OrderService.Common.Contracts.Infrastructure;
using OrderService.Data;
using OrderService.Services.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IMessagesServiceBus, MessagesServiceBus>();
builder.Services.AddScoped<ISendToRabbitMqConsumer, SendToRabbitMqConsumer>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// RabbitMqConnection.Configure(builder.Configuration);
#region Register our context

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
