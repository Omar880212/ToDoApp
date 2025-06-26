using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListApp.Models; 

namespace TodoListApp.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<Models.TodoTask>> GetAllTasksAsync();
        Task<Models.TodoTask> GetTaskByIdAsync(Guid id);
        Task AddTaskAsync(Models.TodoTask task);
        Task UpdateTaskAsync(Models.TodoTask task);
        Task DeleteTaskAsync(Guid id);
    }
}