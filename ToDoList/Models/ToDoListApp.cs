using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoList.Models
{
    public class ToDoListApp : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _id;
        private string _title;
        private string _description;
        private bool _isCompleted;
        private DateTime _createdDate;
        private DateTime _lastModified;
        private bool _isLocked;
        private string _lockedBy;

        public string Id
        {
            get => _id;
            set 
            { 
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Title 
        {
            get => _title;
            set 
            { 
                _title = value;
                OnPropertyChanged();
            }
        }
        public string Description 
        {
            get => _description;
            set 
            { 
                _description = value;
                 OnPropertyChanged();
            }
        }
        public bool IsCompleted
        {
            get => _isCompleted; 
            set
            {
                _isCompleted = value;
                OnPropertyChanged();
                LastModified = DateTime.Now;
            }
        }
        public DateTime CreatedDate
        {
            get => _createdDate;
            set 
            {
                _createdDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime LastModified
        {
            get => _lastModified;
            set
            {
                _lastModified = value;
                OnPropertyChanged();
            }
        }
        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                _isLocked = value;
                OnPropertyChanged();
            }
        }
        public string IsLockedBy
        {
            get => _lockedBy; 
            set
            {
                _lockedBy = value;
                OnPropertyChanged();
            }
        }
        public ToDoListApp()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
            LastModified = DateTime.Now;
        }


        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
