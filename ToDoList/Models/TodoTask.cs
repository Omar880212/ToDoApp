using System;
using System.ComponentModel;

namespace TodoListApp.Models
{
    public class TodoTask : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime LastModified { get; set; }

        public TodoTask()
        {
            Id = Guid.NewGuid();
            LastModified = DateTime.UtcNow;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}