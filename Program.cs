using Microsoft.Azure.Cosmos;
using CosmosEmployeeAPI.Services;
using CosmosEmployeeAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Cosmos DB config
builder.Services.Configure<CosmosDbSettings>(builder.Configuration.GetSection("CosmosDb"));
builder.Services.AddSingleton(s =>
{
    var config = builder.Configuration.GetSection("CosmosDb").Get<CosmosDbSettings>();
    return new CosmosClient(config.Account, config.Key);
});
builder.Services.AddSingleton<CosmosDbService>(s =>
{
    var client = s.GetRequiredService<CosmosClient>();
    var config = builder.Configuration.GetSection("CosmosDb").Get<CosmosDbSettings>();
    return new CosmosDbService(client, config.DatabaseName, config.ContainerName);
});

// ✅ Add Controllers & Swagger
builder.Services.AddEndpointsApiExplorer();  // ✅ Required
builder.Services.AddSwaggerGen(); 
builder.Services.AddControllers()
    .AddNewtonsoftJson();
          // ✅ Required
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Preserve exact property casing
    });
var app = builder.Build();

// ✅ Enable Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseCors("AllowAll");

app.Run("http://0.0.0.0:5215");

