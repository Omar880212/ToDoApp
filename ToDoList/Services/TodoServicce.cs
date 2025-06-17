using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class TodoServicce : IToDoServices
    {
        private static List<ToDoListApp> _tasks = new List<ToDoListApp>();
        private static readonly string CurrentUserId = Environment.UserName;

        public async Task<List<ToDoListApp>> GetAllTaskAsync()
        {
            await Task.Delay(100);
            return new List<ToDoListApp>(_tasks);
        }
        public async Task<ToDoListApp> AddTaskAsync(ToDoListApp task)
        {
            await Task.Delay(100);
            _tasks.Add(task);
            return task;
        }

        public async Task<ToDoListApp> UpdateTaskAsync(ToDoListApp task)
        {
            await Task.Delay(100);
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);

            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.IsCompleted = task.IsCompleted;
                existingTask.LastModified = task.LastModified;
                return existingTask;
            }
            return null;
        }

        public async Task<bool> DeleteTaskAsync(string taskId) 
        {
            await Task.Delay(100);
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null) 
            {
                _tasks.Remove(task);
                return true;
            }

            return false;
        }

        public async Task<bool> LockTaskAsync(string taskId, string userId) 
        {
            await Task.Delay(100);
            var task = _tasks.FirstOrDefault(t =>t.Id == taskId);
            if (task != null && !task.IsLocked) 
            {
                task.IsLocked= true;
                task.IsLockedBy = userId;
                return true;
            }

            return false;
        }

        public async Task<bool> UnlockTaskAsync(string taskId, string UserId) 
        { 
            await Task.Delay(100);
            var task = _tasks.FirstOrDefault(t =>t.Id == taskId);
            if (task != null && task.IsLocked && task.IsLockedBy == UserId)
            {
                task.IsLockedBy = null;
                task.IsLocked = false;

                return true;
            }

            return false;
        }
    }
}
