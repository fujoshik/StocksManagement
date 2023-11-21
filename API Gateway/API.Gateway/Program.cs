using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

    var loggerFactory = LoggerFactory.Create(builder =>
    {
        builder
                .AddConsole();
    });

var logger = loggerFactory.CreateLogger<Program>();

    logger.LogInformation("...");
    logger.LogWarning("Warning");
    logger.LogError("Error");

    
    var user = "Domain";
    logger.LogInformation("Consumer {User} logged in the system.", user);
