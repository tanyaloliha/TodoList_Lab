using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TodoList.Models;

namespace TodoList.Services
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly string _filePath;

        public JsonTaskRepository(string filePath = "tasks.json")
        {
            _filePath = filePath;
        }

        public IEnumerable<TodoModel> LoadTasks()
        {
            if (!File.Exists(_filePath))
                return new List<TodoModel>();

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<TodoModel>>(json) ?? new List<TodoModel>();
            }
            catch
            {
                return new List<TodoModel>();
            }
        }

        public void SaveTasks(IEnumerable<TodoModel> tasks)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(_filePath, json);
        }
    }
}