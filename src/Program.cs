
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(options =>
    {
        options.AddConsoleExporter();
    });
});

var logger = loggerFactory.CreateLogger<Program>();


// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// dynamic page for logger testing
app.MapGet("/Testing", async context =>
{
    logger.LogInformation("Testing logging in Program.cs");
    await context.Response.WriteAsync("Testing");
});

logger.LogInformation($"Welcome to {Assembly.GetEntryAssembly().GetName().Name}.");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

logger.LogInformation("Starting the app");
app.Run();
