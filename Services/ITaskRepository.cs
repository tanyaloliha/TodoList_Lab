using System.Collections.Generic;
using TodoList.Models;

namespace TodoList.Services
{
    public interface ITaskRepository
    {
        IEnumerable<TodoModel> LoadTasks();
        void SaveTasks(IEnumerable<TodoModel> tasks);
    }
}