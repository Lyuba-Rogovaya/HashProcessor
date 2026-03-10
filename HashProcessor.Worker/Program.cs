using HashProcessor.Worker;
using HashProcessor.Worker.Infrastructure;
using HashProcessor.Worker.Services;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddDbContext<MariaDbContext>(options =>
{
    var connectionStrig = builder.Configuration.GetConnectionString("MariaDbConnectionString");
    options.UseMySql(connectionStrig, ServerVersion.AutoDetect(connectionStrig));
});
builder.Services.AddTransient<IProcessHashesService, ProcessHashesService>();
builder.Services.AddTransient<IRabbitMqClient, RabbitMQClient>();

var host = builder.Build();
host.Run();
