using Microsoft.AspNetCore.ResponseCompression;
using Serilog;

Log.Debug("Starting application Builder");
var builder = WebApplication.CreateBuilder(args);

// Add Logger
builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .MinimumLevel.Verbose() //TODO: @Talisi - change to desired level
    .WriteTo.Console()
    .WriteTo.Debug()
    .Enrich.FromLogContext()
    .CreateLogger();
//TODO: Add Azure Application Insights to Serilog Sinks and Configuration
builder.Logging.AddSerilog(logger);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

Log.Debug("Creating the App");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

Log.Debug("Starting the App");
app.Run();
