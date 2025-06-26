using System.Windows;
using TodoListApp.Services;
using TodoListApp.ViewModels;

namespace TodoListApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
