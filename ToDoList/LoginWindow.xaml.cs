using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace TodoListApp
{
    public partial class LoginWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;

        public LoginWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            this.Close();
        }
    }
}