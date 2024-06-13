// Early init of NLog to allow startup and exception logging, before host is built
using Hx.Cache;
using Hx.Sdk.Core;
using Hx.WebApi.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    builder.ConfigureHxWebApp();
    builder.Services.AddCache();
    builder.Services.Configure<TestOptions>(builder.Configuration.GetSection("TestSettings"));
    builder.Services.AddSqlSugar();
    var app = builder.Build();
 
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
  
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });


    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
