using HashProcessor.API.Infrastructure;
using HashProcessor.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MariaDbContext>(options =>
{
    var connectionStrig = builder.Configuration.GetConnectionString("MariaDbConnectionString");
    options.UseMySql(connectionStrig, ServerVersion.AutoDetect(connectionStrig));
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IRabbitMqClient, RabbitMQClient>();
builder.Services.AddTransient<IHashesService, HashesService>();

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
