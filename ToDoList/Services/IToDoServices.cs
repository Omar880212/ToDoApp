using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class IToDoServices
    {
        public interface ITodoServices
        {
            Task<List<ToDoListApp>> GetAllTaskAsync();
            Task<ToDoListApp> AddTaskAsync(ToDoListApp toDoListApp);
            Task<ToDoListApp> UpdateTaskAsync(ToDoListApp toDoListApp);
            Task<bool> DeleteTaskAsync(string taskId);
            Task<bool> LockTaskAsync(string taskId, string userId);
            Task<bool> UnlockTaskAsync(string taskId, string userId);
        }
    }
}
