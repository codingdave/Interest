using Interest.ViewModels;
using Interest.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
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
            // Image size on startup
            // unit tests
            // volltilger
            // Jahre in Main 
            // StartMonth in Main
            // version in about, assemblyinfo.cs
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using IHost host = CreateHostBuilder(e.Args).Build();

            var vm = host.Services.GetService<IMainWindowViewModel>();
            var mw = host.Services.GetService<MainWindow>();
            mw.DataContext = vm;
            mw.ShowDialog();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseConsoleLifetime()
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
                        .AddDebug();
                })
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    serviceCollection
                        .AddTransient<IMainWindowViewModel, MainWindowViewModel>()
                        .AddTransient<MainWindow>()
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
