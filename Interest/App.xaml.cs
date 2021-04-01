using Interest.Logger;
using Interest.Options;
using Interest.ViewModels;
using Interest.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace Interest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // update MWVM: TotalInterest and ResidualDebt whenever the tabs value change
            // scheint so zu sein, dass die _redemptionPercentage und borrowingrate in der UI verwechselt sind
            // create options menu, place language there 
            // classes for percentage, currency, fraction?
            // MainWindow: Years, StartMonth
            // GridView: Show Index for month
            // Splash image, size, position
            // Application Icon
            // obfuscation?
            // Deployment: Zip? Single archive? Trimmed?
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using IHost host = CreateHostBuilder(e.Args).Build();
            var logger = host.Services.GetRequiredService<ILogger<App>>();
            logger.LogInformation("From App. Running the host now..");

            // Start the application with the culture selected
            var configuration = host.Services.GetService<IConfiguration>();
            var options = configuration.Get<Rootobject>();
            if (options?.CultureInfo != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(options.CultureInfo);
            }

            try
            {
                var vm = host.Services.GetService<MainWindowViewModel>();
                var mw = host.Services.GetService<MainWindow>();
                mw.DataContext = vm;
                mw.ShowDialog();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                logger.LogError("Deleting appsettings.json to get rid of invalid configuration");
                System.IO.File.Delete("appsettings.json");
            }

            Current.Shutdown();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
                {
                    IHostEnvironment env = hostBuilderContext.HostingEnvironment;
                    configurationBuilder.Sources.Clear();
                    configurationBuilder
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    IConfigurationRoot configurationRoot = configurationBuilder.Build();

                    foreach ((string key, string value) in configurationRoot.AsEnumerable().Where(t => t.Value is not null))
                    {
                        System.Diagnostics.Debug.WriteLine($"{key}={value}");
                    }
                })
                .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
                {
                    loggingBuilder
                        .SetMinimumLevel(LogLevel.Trace)
                        .AddConfiguration(hostBuilderContext.Configuration.GetSection("Logging"))
                        .AddSimpleConsole(options =>
                        {
                            options.IncludeScopes = true;
                            options.SingleLine = true;
                                //options.TimestampFormat = "hh:mm:ss ";
                                options.UseUtcTimestamp = true;
                        })
                        .AddEventLog()
                        .AddConsole()
                        .AddDebug()
                        .AddFileLogger(options =>
                        {
                            hostBuilderContext.Configuration.GetSection("Logging").GetSection("FileLogger").GetSection("Options").Bind(options);
                        });
                    ;
                })
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    serviceCollection
                        .AddSingleton<MainWindowViewModel>()
                        .AddSingleton<MainWindow>()
                        ;
                    //serviceCollection
                    //.AddTransient<ITransientOperation, TransientOperation>()
                    //.AddScoped<IScopedOperation, ScopedOperation>()
                    //.AddSingleton<ISingletonOperation, SingletonOperation>()
                    //.AddTransient<OperationLogger>();
                })
            ;
    }
}
