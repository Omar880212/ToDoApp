using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TodoListApp.Commands;
using TodoListApp.Models;
using TodoListApp.Services;

namespace TodoListApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ITodoService _todoService;

        private ObservableCollection<Models.TodoTask> _tasks;
        public ObservableCollection<Models.TodoTask> Tasks
        {
            get => _tasks;
            set { _tasks = value; OnPropertyChanged(nameof(Tasks)); }
        }

        private Models.TodoTask _selectedTask;
        public Models.TodoTask SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
                DeleteTaskCommand.RaiseCanExecuteChanged();
                ToggleCompleteCommand.RaiseCanExecuteChanged();
            }
        }

        private string _newTaskTitle;
        public string NewTaskTitle
        {
            get => _newTaskTitle;
            set { _newTaskTitle = value; OnPropertyChanged(nameof(NewTaskTitle)); AddTaskCommand.RaiseCanExecuteChanged(); }
        }

        private string _newTaskDescription;
        public string NewTaskDescription
        {
            get => _newTaskDescription;
            set { _newTaskDescription = value; OnPropertyChanged(nameof(NewTaskDescription)); }
        }

        public RelayCommand AddTaskCommand { get; }
        public RelayCommand DeleteTaskCommand { get; }
        public RelayCommand ToggleCompleteCommand { get; }

        public MainViewModel(ITodoService todoService)
        {
            _todoService = todoService;
            Tasks = new ObservableCollection<Models.TodoTask>();

            AddTaskCommand = new RelayCommand(async () => await AddTask(), () => !string.IsNullOrWhiteSpace(NewTaskTitle));
            DeleteTaskCommand = new RelayCommand(async () => await DeleteTask(), () => SelectedTask != null);
            ToggleCompleteCommand = new RelayCommand(async () => await ToggleComplete(), () => SelectedTask != null);

            _ = LoadTasks();
        }

        private async Task LoadTasks()
        {
            var tasks = await _todoService.GetAllTasksAsync();
            Tasks.Clear();
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
        }

        private async Task AddTask()
        {
            var newTask = new Models.TodoTask
            {
                Title = NewTaskTitle,
                Description = NewTaskDescription
            };
            await _todoService.AddTaskAsync(newTask);
            Tasks.Add(newTask);
            NewTaskTitle = string.Empty;
            NewTaskDescription = string.Empty;
        }

        private async Task DeleteTask()
        {
            if (SelectedTask == null) return;
            await _todoService.DeleteTaskAsync(SelectedTask.Id);
            Tasks.Remove(SelectedTask);
        }

        private async Task ToggleComplete()
        {
            if (SelectedTask == null) return;
            SelectedTask.IsCompleted = !SelectedTask.IsCompleted;
            await _todoService.UpdateTaskAsync(SelectedTask);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}