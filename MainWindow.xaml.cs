using System;
using System.Windows;
using System.Windows.Controls;
using TodoList.Models;
using TaskStatus = TodoList.Models.TaskStatus;
using TodoList.Services;

namespace TodoList
{
    public partial class MainWindow : Window
    {
        private readonly TodoService _todoService;

        public MainWindow()
        {
            InitializeComponent();

            var repository = new JsonTaskRepository("my_tasks.json");
            _todoService = new TodoService(repository);

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            TasksGrid.ItemsSource = null;
            TasksGrid.ItemsSource = _todoService.GetAllTasks();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var priorityItem = PriorityBox.SelectedItem as ComboBoxItem ?? PriorityBox.Items[0] as ComboBoxItem;
                var statusItem = StatusBox.SelectedItem as ComboBoxItem ?? StatusBox.Items[0] as ComboBoxItem;

                var newTask = new TodoModel
                {
                    Title = TitleBox.Text,
                    Description = DescBox.Text,
                    DueDate = DueDatePicker.SelectedDate ?? DateTime.Now.AddDays(1),
                    Priority = (TaskPriority)Enum.Parse(typeof(TaskPriority), priorityItem.Content.ToString()),
                    Status = (TaskStatus)Enum.Parse(typeof(TaskStatus), statusItem.Content.ToString())
                };

                _todoService.AddTask(newTask);
                RefreshGrid();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksGrid.SelectedItem is TodoModel selectedTask)
            {
                try
                {
                    selectedTask.Title = TitleBox.Text;
                    selectedTask.Description = DescBox.Text;
                    selectedTask.DueDate = DueDatePicker.SelectedDate ?? selectedTask.DueDate;

                    var priorityItem = PriorityBox.SelectedItem as ComboBoxItem ?? PriorityBox.Items[0] as ComboBoxItem;
                    var statusItem = StatusBox.SelectedItem as ComboBoxItem ?? StatusBox.Items[0] as ComboBoxItem;

                    selectedTask.Priority = (TaskPriority)Enum.Parse(typeof(TaskPriority), priorityItem.Content.ToString());
                    selectedTask.Status = (TaskStatus)Enum.Parse(typeof(TaskStatus), statusItem.Content.ToString());

                    _todoService.UpdateTask(selectedTask);
                    RefreshGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Выберите задачу в таблице для редактирования.", "Внимание");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksGrid.SelectedItem is TodoModel selectedTask)
            {
                _todoService.DeleteTask(selectedTask.Id);
                RefreshGrid();
                ClearInputs();
            }
            else
            {
                MessageBox.Show("Выберите задачу в таблице для удаления.", "Внимание");
            }
        }

        private void TasksGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TasksGrid.SelectedItem is TodoModel selectedTask)
            {
                TitleBox.Text = selectedTask.Title;
                DescBox.Text = selectedTask.Description;
                DueDatePicker.SelectedDate = selectedTask.DueDate;
                PriorityBox.Text = selectedTask.Priority.ToString();
                StatusBox.Text = selectedTask.Status.ToString();
            }
        }

        private void ClearInputs()
        {
            TitleBox.Clear();
            DescBox.Clear();
            DueDatePicker.SelectedDate = null;
            PriorityBox.SelectedIndex = -1;
            StatusBox.SelectedIndex = -1;
        }
    }
}