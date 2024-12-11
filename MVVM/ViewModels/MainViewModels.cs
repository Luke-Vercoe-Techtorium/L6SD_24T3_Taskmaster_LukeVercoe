using PropertyChanged;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using TaskManager.MVVM.Models;
using TaskManager.Services;

namespace TaskManager.MVVM.ViewModels;

[AddINotifyPropertyChangedInterface]
public class MainViewModels
{
    public Database Database { get; set; }

    public ObservableCollection<CategoryModel> Categories { get; set; } = [];
    public ObservableCollection<TaskModel> Tasks { get; set; } = [];

    public MainViewModels(Database database)
    {
        Database = database;

        Categories.CollectionChanged += Categories_CollectionChanged;
        Tasks.CollectionChanged += Tasks_CollectionChanged;

        LoadData();
    }

    private void Categories_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateData();
    }

    private void Tasks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateData();
    }

    private async void LoadData()
    {
        var categoriesFromDb = await Database.GetCategoriesAsync();
        foreach (var category in categoriesFromDb)
            Categories.Add(category);

        var tasksFromDb = await Database.GetTasksAsync();
        foreach (var task in tasksFromDb)
            Tasks.Add(task);

        UpdateData();
    }

    public void UpdateData()
    {
        foreach (var category in Categories)
        {
            var tasks = Tasks.Where(task => task.CategoryID == category.Id);
            var completed = tasks.Where(task => task.Completed).Count();
            var totalTasks = tasks.Count();

            category.Completed = completed;
            category.PendingTasks = totalTasks - completed;
            category.Percentage = totalTasks == 0 ? 0 : (float)completed / totalTasks;
        }

        foreach (var task in Tasks)
        {
            var catColor = Categories.FirstOrDefault(category => category.Id == task.CategoryID)?.Color;
            if (catColor is not null)
                task.TaskColor = catColor;
        }
    }

    public async Task DeleteTaskAsync(TaskModel task)
    {
        if (task is null) return;
        await Database.DeleteTaskAsync(task);
        Tasks.Remove(task);
        UpdateData();
    }
}
