using AIpazz.Infrastructure.Billing;
using Aipazz.Application.Billing.TimeEntries.Queries;
using Aipazz.Application.Calender.Interface;
using Aipazz.Application.Calender.Interfaces;
using Microsoft.Azure.Cosmos;
using Aipazz.Domian;
using Microsoft.Extensions.Options;
using Aipazz.Application.DocumentMGT.Interfaces;
using AIpazz.Infrastructure.Calender;
using AIpazz.Infrastructure.Documentmgt;
using Aipazz.Infrastructure.Matters;
using Aipazz.Infrastructure.Billing;
using AIpazz.Infrastructure.Calendar;
using AIpazz.Infrastructure.Documentmgt.Services;
using AIpazz.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Azure.Storage.Blobs;
using System.Text.Json;
using Aipazz.Application.Common.Aipazz.Application.Common;
using Aipazz.Application.Billing.Interfaces;
using AIpazz.Infrastructure.Billing.Aipazz.Application.Common;
using Aipazz.Infrastructure.Calender;
using QuestPDF.Infrastructure;



var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddAuthorization();


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

builder.Services.AddMatterServices(builder.Configuration);





builder.Services.AddInfrastructureServices();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserContext, UserContext>();





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




builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IdocumentRepository, DocumentRepository>();
builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();
builder.Services.AddScoped<IclientmeetingRepository, Clientmeetingrepository>();
builder.Services.AddScoped<ICourtDateFormRepository, CourtDateFormRepository>();
builder.Services.AddScoped<IFilingsDeadlineFormRepository, FilingsDeadlineFormRepository>();
builder.Services.AddScoped<ITeamMeetingFormRepository, TeamMeetingFormRepository>();
    

builder.Services.AddSingleton(x =>
    new BlobServiceClient(builder.Configuration["AzureBlob:ConnectionString"])
);
builder.Services.AddScoped<IFileStorageService, AzureBlobStorageService>();
builder.Services.AddScoped<IInvoicePdfGenerator, InvoicePdfGenerator>();
builder.Services.Configure<InvoiceBlobOptions>(
    builder.Configuration.GetSection("InvoiceBlob"));


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Allow frontend URL
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
builder.Services.AddScoped<IInvoiceBlobService, AzureInvoiceBlobService>();



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
