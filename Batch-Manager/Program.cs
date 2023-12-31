using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Batch_Manager.DatabaseContext;
using Batch_Manager.Services;
using Batch_Manager.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Read DbConnectionString from appsettings configuration file
//builder.Services.AddDbContext<BatchContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

//Configure KeyVault and read sql server connectionString from keyvault secret
var kvUrl = builder.Configuration.GetValue<string>("AzureKeyVaultUrl");
builder.Configuration.AddAzureKeyVault(
        new Uri(kvUrl.ToString()),
        new DefaultAzureCredential());
var secretClient = new SecretClient(new Uri(kvUrl.ToString()), new DefaultAzureCredential());
var sqlConnectionString = secretClient.GetSecret("SqlConnectionString");
builder.Services.AddDbContext<BatchContext>(x => x.UseSqlServer((sqlConnectionString.Value).Value));

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IBatch, BatchService>();
builder.Services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
builder.Services.AddScoped<IBlobStorageClient, BlobStorageClient>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Batch Manager API",
        Description = "Batch manager API to create batch and get batch details."
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddValidatorsFromAssemblyContaining<BatchValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IBatchValidator, BatchValidator>();
var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
