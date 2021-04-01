using Interest.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Interest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using IHost host = CreateHostBuilder(e.Args).Build();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
                {
                    configurationBuilder.Sources.Clear();

                    IHostEnvironment env = hostingContext.HostingEnvironment;

                    configurationBuilder
                        .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddIniFile($"appsettings.ini", optional: true, reloadOnChange: true)
                        .AddIniFile($"appsettings.{env.EnvironmentName}.ini", optional: true, reloadOnChange: true)
                        .AddXmlFile($"appsettings.xml", optional: true, reloadOnChange: true)
                        .AddXmlFile($"repeating-example.xml", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables(prefix: "CustomPrefix_") 
                    ;

                    IConfigurationRoot configurationRoot = configurationBuilder.Build();

                    //TransientFaultHandlingOptions transientOptions = new();
                    //configurationRoot.GetSection(nameof(TransientFaultHandlingOptions)).Bind(transientOptions);
                    //System.Diagnostics.Debug.WriteLine($"TransientFaultHandlingOptions.Enabled={transientOptions.Enabled}");
                    //System.Diagnostics.Debug.WriteLine($"TransientFaultHandlingOptions.AutoRetryDelay={transientOptions.AutoRetryDelay}");

                    foreach ((string key, string value) in
                        configurationRoot.AsEnumerable().Where(t => t.Value is not null))
                    {
                        System.Diagnostics.Debug.WriteLine($"{key}={value}");
                    }
                });

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //throw new NotImplementedException();
            //containerRegistry.RegisterSingleton<ApplicationService>();
            //containerRegistry.RegisterSingleton<CoreRoutines>();
            //containerRegistry.RegisterSingleton<DialogService>();

            // Configure Serilog and the sinks at the startup of the app
            //var logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .WriteTo.Console()
            //    .WriteTo.File(path: "MyApp.log", encoding: Encoding.UTF8)
            //    .CreateLogger();
            // Register Serilog with Prism
            //containerRegistry.RegisterSerilog(logger);
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
    }

}
