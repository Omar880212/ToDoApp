using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using ToDoList.Commands;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly TodoServicce _todoServices;
        private ToDoListApp _selectedTask;
        private ToDoListApp _taskBeinEdited;
        private string _newTaskTitle;
        private string _newTaskDescription;
        private bool _isAddingTask;

        public int CompletedTaskCount => Tasks?.Count(t => t.IsCompleted) ?? 0;
        public int PendingTaskCount => Tasks?.Count(t => t.IsCompleted) ?? 0;
        public ObservableCollection<ToDoListApp> Tasks { get; set; }
        public ToDoListApp SelectedTask 
        { 
            get => _selectedTask;
            set 
            {
                _selectedTask = value;
                OnPropertyChange();
                EditTaskCommand?.RaiseCanExecuteChanged();
                DeleteTaskCommand?.RaiseCanExecuteChanged();
                ToggleCompleteCommand?.RaiseCanExecuteChanged();
            } 
        }        
        public ToDoListApp TaskBeingEdited
        {
            get => _taskBeinEdited;
            set 
            {
                _taskBeinEdited= value;
                OnPropertyChange();
                OnPropertyChange(nameof(IsEditingTask));
            }
        }
        public string NewTaskTitle 
        {
            get => _newTaskTitle;
            set 
            {
                _newTaskTitle= value;
                OnPropertyChange();
                AddTaskCommand.RaiseCanExecuteChanged();
            }
        }
        public string NewTaskDescription 
        {
            get => _newTaskDescription;
            set 
            {
                _newTaskDescription= value;
                OnPropertyChange();
            }
        }
        public bool IsAddingTask 
        {
            get => _isAddingTask; 
            set
            {
                _isAddingTask = value;
                OnPropertyChange();
            }
        }
        public bool IsEditingTask => TaskBeingEdited!= null;
        public RelayCommand AddTaskCommand { get; }
        public RelayCommand EditTaskCommand { get; }
        public RelayCommand SaveTaskCommand { get; }
        public RelayCommand CancelEditCommand { get; }
        public RelayCommand DeleteTaskCommand { get; }
        public RelayCommand ToggleCompleteCommand { get; }
        public MainViewModel(IToDoServices toDoServices)
        {
            _todoServices = (TodoServicce)toDoServices;
            Tasks = new ObservableCollection<ToDoListApp>();

            //Tasks.CollectionChanged += (s, e) =>
            //{
            //    OnPropertyChange(nameof(CompletedTaskCount));
            //    OnPropertyChange(nameof(PendingTaskCount));
            //};

            AddTaskCommand = new RelayCommand(async () => await AddTask(), () => !string.IsNullOrEmpty(NewTaskTitle));
            EditTaskCommand = new RelayCommand(async () => await EditTask(), () => SelectedTask != null && !SelectedTask.IsLocked);
            SaveTaskCommand = new RelayCommand(async () => await SaveTask());
            CancelEditCommand = new RelayCommand(CancelEdit);
            DeleteTaskCommand = new RelayCommand(async () => await DeleteTask(), () => SelectedTask != null && !SelectedTask.IsLocked);
            ToggleCompleteCommand = new RelayCommand(async () => await ToggleComplete(), () => SelectedTask != null && !SelectedTask.IsLocked);

            _ = LoadTasks();
        }
        private async Task LoadTasks()
        {
            try
            {
                var tasks = await _todoServices.GetAllTaskAsync();
                Tasks.Clear();

                foreach (var task in tasks) 
                {
                    Tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task AddTask()
        {
            if (string.IsNullOrEmpty(NewTaskTitle))
            {
                return;
            }

            try
            {
                var newTask = new ToDoListApp
                {
                    Title = NewTaskTitle.Trim(),
                    Description = NewTaskDescription.Trim() ?? string.Empty,
                };

                var addedTask = await _todoServices.AddTaskAsync(newTask);
                Tasks.Add(addedTask);

                NewTaskTitle = string.Empty;
                NewTaskDescription = string.Empty;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task EditTask()
        {
            if (SelectedTask == null)
            {
                return;
            }
            try
            {
                var locked = await _todoServices.LockTaskAsync(SelectedTask.Id, Environment.UserName);

                if (locked) 
                {
                    TaskBeingEdited = new ToDoListApp
                    {
                        Id = SelectedTask.Id,
                        Title = SelectedTask.Title,
                        Description = SelectedTask.Description,
                        IsCompleted = SelectedTask.IsCompleted,
                        CreatedDate = SelectedTask.CreatedDate,
                        LastModified = SelectedTask.LastModified
                    };
                }
                else
                {
                    MessageBox.Show($"This task is currently bein edited by another user.", "Task locked", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task SaveTask()
        {
            if (TaskBeingEdited == null)
            {
                return;
            }
            try
            {
                var updateTask = await _todoServices.UpdateTaskAsync(TaskBeingEdited);
                if (updateTask != null)
                {
                    var existingTask = Tasks.FirstOrDefault(t => t.Id == updateTask.Id);
                    if (existingTask != null) 
                    {
                        existingTask.Title = updateTask.Title;
                        existingTask.Description = updateTask.Description;
                        existingTask.IsCompleted = updateTask.IsCompleted;
                        existingTask.LastModified = updateTask.LastModified;
                    }

                    await _todoServices.UnlockTaskAsync(TaskBeingEdited.Id, Environment.UserName);
                    TaskBeingEdited = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void CancelEdit()
        {
            if (TaskBeingEdited != null)
            {
                try
                {
                    await _todoServices.UnlockTaskAsync(TaskBeingEdited.Id, Environment.UserName);
                    TaskBeingEdited = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error canceling edit: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async Task DeleteTask()
        {
            if (SelectedTask == null)
            {
                return;
            }
            try
            {
                var result = MessageBox.Show($"Are you sure you want to delete '{SelectedTask.Title}'?",
                                              "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes) 
                {
                    var deleted = await _todoServices.DeleteTaskAsync(SelectedTask.Id);
                    if (deleted) 
                    {
                        Tasks.Remove(SelectedTask);
                        SelectedTask = null;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting edit: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task ToggleComplete()
        {
            if (SelectedTask == null)
            {
                return;
            }
            try
            {
                SelectedTask.IsCompleted = !SelectedTask.IsCompleted;
                await _todoServices.UpdateTaskAsync(SelectedTask);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updatung edit: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
