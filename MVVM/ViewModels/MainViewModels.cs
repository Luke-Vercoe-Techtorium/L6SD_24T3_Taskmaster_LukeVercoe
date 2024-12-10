using PropertyChanged;
using System.Collections.ObjectModel;
using TaskManager.MVVM.Models;
using TaskManager.Services;

namespace TaskManager.MVVM.ViewModels;

[AddINotifyPropertyChangedInterface]
public class MainViewModels
{

    public Database Database { get; set; }

    public ObservableCollection<CategoryModel> Categories { get; set; }
    public ObservableCollection<TaskModel> Tasks { get; set; }

    public MainViewModels(Database database)
    {
        Database = database;

        Categories = [];
        Tasks = [];
        Tasks.CollectionChanged += Tasks_CollectionChanged;

        LoadData();
    }


    private void Tasks_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        UpdateData();
    }

    private async void LoadData()
    {
        var categoriesFromDb = await Database.GetCategoriesAsync();
        foreach (var category in categoriesFromDb)
        {
            Categories.Add(category);
        }

        var tasksFromDb = await Database.GetTasksAsync();
        foreach (var task in tasksFromDb)
        {
            Tasks.Add(task);
        }

        UpdateData();
    }

    public void UpdateData()
    {
        foreach (var c in Categories)
        {
            var tasks = Tasks.Where(t => t.CategoryID == c.Id);
            var completed = tasks.Where(t => t.Completed).Count();
            var totalTasks = tasks.Count();

            c.Completed = completed;
            c.PendingTasks = totalTasks - completed;
            c.Percentage = totalTasks == 0 ? 0 : (float)completed / totalTasks;
        }
        foreach (var t in Tasks)
        {
            var catColor = Categories.FirstOrDefault(c => c.Id == t.CategoryID)?.Color;
            if (catColor is not null)
                t.TaskColor = catColor;
        }
    }

    public async Task DeleteTaskAsync(TaskModel task)
    {
        if (task == null) return;
        await Database.DeleteTaskAsync(task);
        Tasks.Remove(task);
        UpdateData();
    }
}