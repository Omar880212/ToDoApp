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
        private readonly IToDoServices _todoServices;
        private ToDoListApp _selectedTask;
        private ToDoListApp _taskBeinEdited;
        private string _newTaskTitle;
        private string _newTaskDescription;
        private bool _isAddingTask;

        public ObservableCollection<ToDoListApp> Tasks { get; set; }

        public ToDoListApp SelectedTask 
        { 
            get => _selectedTask;
            set 
            {
                _selectedTask = value;
                OnPropertyChange();
                EditTaskCommand.RaiseCanExecuteChanged();
                DeleteTaskCommand.RaiseCanExecuteChanged();
                ToggleCompleteCommand.RaiseCanExecuteChanged();
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
            _todoServices = toDoServices;
            Tasks = new ObservableCollection<ToDoListApp>();

            AddTaskCommand = new RelayCommand(async () => await AddTask(), () => !string.IsNullOrEmpty(NewTaskTitle));
            EditTaskCommand = new RelayCommand(async () => await EditTask(), () => SelectedTask != null && !SelectedTask.IsLocked);
            SaveTaskCommand = new RelayCommand(async () => await SaveTask());
            CancelEditCommand = new RelayCommand(CancelEdit);
            DeleteTaskCommand = new RelayCommand(async () => await DeleteTask(), () => SelectedTask != null && !SelectedTask.IsLocked);
            ToggleCompleteCommand = new RelayCommand(async () => await ToggleComplete(), () => SelectedTask != null && !SelectedTask.IsLocked);
        }        

        private Task AddTask()
        {
            throw new NotImplementedException();
        }
        private Task EditTask()
        {
            throw new NotImplementedException();
        }
        private Task SaveTask()
        {
            throw new NotImplementedException();
        }
        private void CancelEdit()
        {
            throw new NotImplementedException();
        }
        private Task DeleteTask()
        {
            throw new NotImplementedException();
        }
        private Task ToggleComplete()
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
