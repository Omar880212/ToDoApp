using System.Windows;
using ToDoList.Services;
using ToDoList.ViewModels;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                var todoservice = new TodoServicce();
                var viewModel = new MainViewModel(todoservice);
                DataContext = viewModel;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error: { ex.Message }","Error",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
