using AIpazz.Infrastructure.Billing;
using Aipazz.Application.Billing.TimeEntries.Queries;
using Microsoft.Azure.Cosmos;
using Aipazz.Domian;
using Microsoft.Extensions.Options;
using Aipazz.Application.DocumentMGT.Interfaces;
using AIpazz.Infrastructure.Documentmgt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllTimeEntriesQuery).Assembly));

// Bind CosmosDbOptions
builder.Services.Configure<CosmosDbOptions>(
    builder.Configuration.GetSection("CosmosDb")
);

// Register CosmosClient
builder.Services.AddSingleton<CosmosClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<CosmosDbOptions>>().Value;

    if (string.IsNullOrEmpty(options.AccountEndpoint) || string.IsNullOrEmpty(options.AuthKey))
    {
        throw new InvalidOperationException("Cosmos DB connection details are not configured properly.");
    }

    return new CosmosClient(options.AccountEndpoint, options.AuthKey);
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
builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
