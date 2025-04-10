using Aipazz.Application;
using AIpazz.Infrastructure.Billing;
using Aipazz.Application.Billing.TimeEntries.Queries;
using AIpazz.Infrastructure;
using Microsoft.Azure.Cosmos;
using Aipazz.Application.DocumentMGT.Interfaces;
using AIpazz.Infrastructure.Documentmgt;
using Aipazz.Application.DocumentMGT.documentmgt.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllTimeEntriesQuery).Assembly));


// Register Cosmos DB connection
builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var configuration = builder.Configuration;
    string? accountEndpoint = configuration["CosmosDb:AccountEndpoint"];
    string? authKey = configuration["CosmosDb:AuthKey"];

    if (string.IsNullOrEmpty(accountEndpoint) || string.IsNullOrEmpty(authKey))
    {
        throw new InvalidOperationException("Cosmos DB connection details are not configured properly.");
    }

    return new CosmosClient(accountEndpoint, authKey);
});

builder.Services.AddBillingServices(builder.Configuration);


// Register CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()   // Allow all origins
                  .AllowAnyMethod()   // Allow all HTTP methods (GET, POST, PUT, DELETE, etc.)
                  .AllowAnyHeader();  // Allow all headers
        });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IdocumentRepository, DocumentRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
