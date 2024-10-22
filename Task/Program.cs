using Microsoft.Extensions.Configuration;
using Task.Application.Interfaces;
using Task.Application.MappingProfile;
using Task.Application.Services;
using Task.Domain.Interfaces.Repository;
using Task.Infrastructure.Options;
using Task.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(IMapper));

builder.Services.Configure<FileOrderRepositoryOptions>(builder.Configuration.GetSection("FileOrderRepositoryOptions"));
builder.Services.AddScoped<IOrderRepository, FileOrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();


builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMqOptions"));

builder.Services.AddSingleton<ILoggerService, RabbitMqLogger>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
