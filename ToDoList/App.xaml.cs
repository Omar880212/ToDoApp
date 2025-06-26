using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;
using TodoListApp.Data;
using TodoListApp.Services;
using TodoListApp.ViewModels;

namespace TodoListApp
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    string connectionString = context.Configuration.GetConnectionString("DefaultConnection");

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(connectionString));

                    services.AddTransient<ITodoService, TodoService>();
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<MainWindow>();

                    // ¡ASEGÚRATE DE QUE ESTA LÍNEA ESTÉ AQUÍ!
                    services.AddTransient<LoginWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            
            var loginWindow = _host.Services.GetRequiredService<LoginWindow>();
            loginWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}