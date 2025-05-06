using MassTransit;
using TwoServices.Controllers;
using TwoServices.Setting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(); // Add HttpClient service

builder.Services.Configure<RabbitMQSetting>(builder.Configuration.GetSection("RabbitMQSettings"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CustomerCreatedConsumer>();
    

    x.UsingRabbitMq((context, configurator) =>
    {
        // קבלת RabbitMQSetting מתוך הקונפיגורציה
        var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQSettings").Get<RabbitMQSetting>();

        configurator.Host(rabbitMqSettings.host); // העברת הכתובת של ה-RabbitMQ
        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("TwoServices.Setting", false));
    });
});
builder.Services.AddMassTransitHostedService();
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
