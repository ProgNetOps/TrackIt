using TrackIt.Presentation.ServicesExtension;
using NLog;
using NLog.Web;

//Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{    
    var builder = WebApplication.CreateBuilder(args);

    //NLog: Setup NLog for Dependency injection

    builder.Logging.ClearProviders();
 
    builder.Host.UseNLog();

    builder.Services.ConfigureServices(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment() is false)
    {
        //FOR UNHANDLED EXCEPTIONS
        app.UseExceptionHandler("/Error");

        //STATUS CODE MIDDLEWARE COMPONENTS
        app.UseStatusCodePagesWithReExecute("/Error/{0}");

        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    else
    {
        app.UseDeveloperExceptionPage();

    }

    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

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