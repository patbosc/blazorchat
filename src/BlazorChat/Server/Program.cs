using BlazorChat.Server;
using Microsoft.AspNetCore.Cors.Infrastructure;
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



// set the CORS policy - endpointroting with named policy 
string[] origins = new[] { "https://localhost:7161" };

var corsOriginsEnvVar = Environment.GetEnvironmentVariable("CORS_ORIGINS");
if (!string.IsNullOrEmpty(corsOriginsEnvVar))
{
    origins = corsOriginsEnvVar.Split(";");
}
Log.Debug("CORS_ORIGINS: {CORS_ORIGINS}", origins);



builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "SignalRPolicy", policy =>
    {
        policy
            .WithOrigins(origins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
    });
});


Log.Debug("Adding SignalR");
builder.Services.AddSignalR();

Log.Debug("Creating the App");
var app = builder.Build();

app.UseSerilogRequestLogging();

app.Use(async (context, next) =>
{
    // Log/Print all Headers
    foreach (var header in context.Request.Headers)
    {
        Log.Information("Header: {Key}: {Value}", header.Key, header.Value);
    }

    Log.Information("Request Method: {Method}", context.Request.Method);
    Log.Information("Request Scheme: {Scheme}", context.Request.Scheme);
    Log.Information("Request Path: {Path}", context.Request.Path);

    await next();
});

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

app.UseAuthorization();
app.UseCors("SignalRPolicy");


app.MapRazorPages();
app.MapControllers();
app.Map("/", () => $"Hello World! {Environment.GetEnvironmentVariable("POD_NAME")}"  );
app.MapHub<ChatHub>("/chathub");
app.MapFallbackToFile("index.html");

Log.Debug("Starting the App");
app.Run();
