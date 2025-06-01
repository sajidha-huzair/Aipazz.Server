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

using Aipazz.Application.DocumentMGT.documentmgt.Queries;
using Aipazz.Infrastructure.Calendar;
using AIpazz.Infrastructure.Calendar;
using AIpazz.Infrastructure.Documentmgt.Services;
using AIpazz.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Azure.Storage.Blobs;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Aipazz.Infrastructure.Matters;
using Aipazz.Application.Matters.Interfaces;



var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<IclientmeetingRepository, Clientmeetingrepository>();
builder.Services.AddSingleton<ICourtDateFormRepository, CourtDateFormRepository>();
builder.Services.AddSingleton<IFilingsDeadlineFormRepository, FilingsDeadlineFormRepository>();
builder.Services.AddSingleton<ITeamMeetingFormRepository, TeamMeetingFormRepository>();



builder.Services.AddSingleton(x =>
    new BlobServiceClient(builder.Configuration["AzureBlob:ConnectionString"])
);
builder.Services.AddScoped<IFileStorageService, AzureBlobStorageService>();





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
