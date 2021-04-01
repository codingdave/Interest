using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;

namespace Interest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Logger _logger;

        public App()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, fileSizeLimitBytes: null)
                .CreateLogger();

            _logger.Information("Application started");
            _logger.Information("GetFolderPath: {0}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using IHost host = CreateHostBuilder(e.Args).Build();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();

                    IHostEnvironment env = hostingContext.HostingEnvironment;

                    configuration
                        .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddIniFile($"appsettings.ini", optional: true, reloadOnChange: true)
                        .AddIniFile($"appsettings.{env.EnvironmentName}.ini", optional: true, reloadOnChange: true)
                        .AddXmlFile($"appsettings.xml", optional: true, reloadOnChange: true)
                        .AddXmlFile($"repeating-example.xml", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables(prefix: "CustomPrefix_") 
                    ;

                    IConfigurationRoot configurationRoot = configuration.Build();

                    //TransientFaultHandlingOptions transientOptions = new();
                    //configurationRoot.GetSection(nameof(TransientFaultHandlingOptions)).Bind(transientOptions);
                    //System.Diagnostics.Debug.WriteLine($"TransientFaultHandlingOptions.Enabled={transientOptions.Enabled}");
                    //System.Diagnostics.Debug.WriteLine($"TransientFaultHandlingOptions.AutoRetryDelay={transientOptions.AutoRetryDelay}");

                    foreach ((string key, string value) in
                        configuration.Build().AsEnumerable().Where(t => t.Value is not null))
                    {
                        System.Diagnostics.Debug.WriteLine($"{key}={value}");
                    }
                });
    }

}
