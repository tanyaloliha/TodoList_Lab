using System;
using System.Collections.Generic;
using System.Linq;
using TodoList.Models;

namespace TodoList.Services
{
    public class TodoService
    {
        private readonly ITaskRepository _repository;
        private List<TodoModel> _tasks;

        public TodoService(ITaskRepository repository)
        {
            _repository = repository;
            _tasks = new List<TodoModel>(_repository.LoadTasks());
        }

        public IEnumerable<TodoModel> GetAllTasks() => _tasks;

        public void AddTask(TodoModel task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Название задачи не может быть пустым.");

            _tasks.Add(task);
            Save();
        }

        public void UpdateTask(TodoModel updatedTask)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (task != null)
            {
                task.Title = updatedTask.Title;
                task.Description = updatedTask.Description;
                task.DueDate = updatedTask.DueDate;
                task.Priority = updatedTask.Priority;
                task.Status = updatedTask.Status;
                Save();
            }
        }

        public void DeleteTask(Guid taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                _tasks.Remove(task);
                Save();
            }
        }

        public void Save()
        {
            _repository.SaveTasks(_tasks);
        }
    }
}