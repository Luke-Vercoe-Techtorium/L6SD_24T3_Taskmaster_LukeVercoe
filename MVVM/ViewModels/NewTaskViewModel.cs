using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.MVVM.Models;
using TaskManager.Services;

namespace TaskManager.MVVM.ViewModels
{
    public class NewTaskViewModel
    {
        private readonly Database _database;

        public string Task { get; set; } = string.Empty;
        public ObservableCollection<TaskModel> Tasks { get; set; }
        public ObservableCollection<CategoryModel> Categories { get; set; }

        public NewTaskViewModel(Database database, ObservableCollection<CategoryModel> categories, ObservableCollection<TaskModel> tasks)
        {
            _database = database;
            Categories = categories;
            Tasks = tasks;
        }

        public async Task AddTaskAsync(TaskModel task)
        {
            await _database.SaveTaskAsync(task);
            Tasks.Add(task);
        }

        public async Task DeleteTaskAsync(TaskModel task)
        {
            await _database.DeleteTaskAsync(task);
            Tasks.Remove(task);
        }

        public async Task AddCategoryAsync(CategoryModel category)
        {
            await _database.SaveCategoryAsync(category);
            Categories.Add(category);
        }

        public async Task DeleteCategoryAsync(CategoryModel category)
        {
            var tasksToDelete = Tasks.Where(t => t.CategoryID == category.Id).ToList();
            foreach (var task in tasksToDelete)
            {
                await DeleteTaskAsync(task);
            }

            await _database.DeleteCategoryAsync(category);
            Categories.Remove(category);
        }
    }
}
