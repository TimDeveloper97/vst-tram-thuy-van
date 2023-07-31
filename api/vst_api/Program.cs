using Serilog;
using share;
using System.Reflection;
using vst_api.Contracts;
using vst_api.Repository;

InitDB();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// logger
builder.Host.UseSerilog((context, logger) => logger.WriteTo.Console().ReadFrom.Configuration(context.Configuration));

// DI
builder.Services.AddScoped(typeof(IAuthRepository), typeof(AuthRepository));





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
    string basePath = Directory.GetParent(currentPath).Parent.Parent.Parent.FullName;
    string appDataPath = basePath + "\\app_data";

    // init db
    DB.Register(appDataPath);
}