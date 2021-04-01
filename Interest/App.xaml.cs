using DependencyInjection.Example;
using Interest.ViewModels;
using Interest.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Interest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // IOC
            // Configuration bind
            // Load/Save Json
            // Image size on startup
            // save per tab
            // unit tests
            //var c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //var r = new ConfigurationManagerReaderWriter(c);
            //var w = new MainWindow();
            //w.DataContext = new MainWindowViewModel(r);
            //w.WindowState = WindowState.Maximized;
            //w.WindowStyle = WindowStyle.SingleBorderWindow;
            //w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //w.ShowDialog();

        //    var appConfig = new AppConfig()
        //    {
        //        //your appsettings.json props here
        //    }
        //    _appConfiguration.Bind(appConfig);
        //    appConfig.IsFirstStart = "...";
        //    string json = JsonConvert.SerializeObject(appConfig);
        //    System.IO.File.WriteAllText("appsettings.json", json);
        // BIND!!!
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using IHost host = CreateHostBuilder(e.Args).Build();

            //_ = host.Services.GetService<ExampleService>();
            host.Run();
            //return host.RunAsync();
        }

        //static Task Main(string[] args) =>
        //    CreateHostBuilder(args).Build().RunAsync();

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddHostedService<Worker>()
                            .AddScoped<IMessageWriter, LoggingMessageWriter>());

        //static IHostBuilder CreateHostBuilder(string[] args) =>
        //    //        services.AddHostedService<Worker>()
        //    //                .AddScoped<IMessageWriter, MessageWriter>());

        //Host.CreateDefaultBuilder(args)
        //        .ConfigureAppConfiguration((hostingContext, services) =>
        //        {
        //            services.Sources.Clear();

        //            IHostEnvironment env = hostingContext.HostingEnvironment;

        //            services
        //                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
        //                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
        //                .AddEnvironmentVariables(prefix: "CustomPrefix_")
        //            ;

        //            IConfigurationRoot configurationRoot = services.Build();

        //            //TransientFaultHandlingOptions transientOptions = new();
        //            //configurationRoot.GetSection(nameof(TransientFaultHandlingOptions)).Bind(transientOptions);
        //            //System.Diagnostics.Debug.WriteLine($"TransientFaultHandlingOptions.Enabled={transientOptions.Enabled}");
        //            //System.Diagnostics.Debug.WriteLine($"TransientFaultHandlingOptions.AutoRetryDelay={transientOptions.AutoRetryDelay}");

        //            foreach ((string key, string value) in
        //                configurationRoot.AsEnumerable().Where(t => t.Value is not null))
        //            {
        //                System.Diagnostics.Debug.WriteLine($"{key}={value}");
        //            }
        //        });

        //protected override void RegisterTypes(IContainerRegistry containerRegistry)
        //{
        //    //containerRegistry.RegisterSingleton<ApplicationService>();
        //    //containerRegistry.RegisterSingleton<CoreRoutines>();
        //    //containerRegistry.RegisterSingleton<DialogService>();

        //    // Configure Serilog and the sinks at the startup of the app
        //    //var logger = new LoggerConfiguration()
        //    //    .MinimumLevel.Debug()
        //    //    .WriteTo.Console()
        //    //    .WriteTo.File(path: "MyApp.log", encoding: Encoding.UTF8)
        //    //    .CreateLogger();
        //    // Register Serilog with Prism
        //    //containerRegistry.RegisterSerilog(logger);

        //    containerRegistry
        //        .RegisterSingleton<Configuration>(() =>
        //        {
        //            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //        })
        //        .RegisterSingleton<ConfigurationManagerReaderWriter>();
        //}

        //protected override Window CreateShell()
        //{
        //    var vm = Container.Resolve<MainWindowViewModel>();
        //    var w = Container.Resolve<MainWindow>();
        //    w.DataContext = vm;
        //    w.WindowState = WindowState.Maximized;
        //    w.WindowStyle = WindowStyle.SingleBorderWindow;
        //    w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //    return w;
        //}
    }

}
