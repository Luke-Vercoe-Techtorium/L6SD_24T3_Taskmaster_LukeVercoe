using System.Collections.ObjectModel;
using TaskManager.MVVM.Models;
using TaskManager.Services;

namespace TaskManager.MVVM.ViewModels;

public class NewTaskViewModel(Database database, ObservableCollection<CategoryModel> categories, ObservableCollection<TaskModel> tasks)
{
    public string Task { get; set; } = string.Empty;
    public ObservableCollection<TaskModel> Tasks { get; set; } = tasks;
    public ObservableCollection<CategoryModel> Categories { get; set; } = categories;

    public async Task AddTaskAsync(TaskModel task)
    {
        await database.SaveTaskAsync(task);
        Tasks.Add(task);
    }

    public async Task DeleteTaskAsync(TaskModel task)
    {
        await database.DeleteTaskAsync(task);
        Tasks.Remove(task);
    }

    public async Task AddCategoryAsync(CategoryModel category)
    {
        await database.SaveCategoryAsync(category);
        Categories.Add(category);
    }

    public async Task DeleteCategoryAsync(CategoryModel category)
    {
        var tasksToDelete = Tasks.Where(t => t.CategoryID == category.Id).ToList();
        foreach (var task in tasksToDelete)
        {
            await DeleteTaskAsync(task);
        }

        await database.DeleteCategoryAsync(category);
        Categories.Remove(category);
    }
}
