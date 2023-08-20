using Microsoft.OpenApi.Models;
using Serilog;
using share;
using System.Reflection;
using vst_api.Configurations;
using vst_api.Contracts;
using vst_api.Repository;

InitDB();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //doc
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Tram Thuy Van API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Tim Developer",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Tim Developer",
            Url = new Uri("https://example.com/license")
        }
    });

    options.OperationFilter<SwaggerBodyTypeOperationFilter>();
    // Set the comments path for the Swagger JSON and UI.
    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //options.IncludeXmlComments(xmlPath);
});

// logger
builder.Host.UseSerilog((context, logger) => logger.WriteTo.Console().ReadFrom.Configuration(context.Configuration));

// DI
builder.Services.AddTransient(typeof(IGenericDynamicRepository<>), typeof(GenericDynamicRepository<>));
builder.Services.AddTransient(typeof(IGenericStaticRepository<>), typeof(GenericStaticRepository<>));

/// <summary>
/// ////////////////////////////////////////////////////////////////////////////////////////////////
/// </summary>
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

void InitDB()
{
    string currentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    string basePath = Directory.GetParent(currentPath).Parent.Parent.Parent.Parent.FullName;
    string appDataPath = basePath + "\\app_data";

    // init db
    DB.Register(appDataPath);
}