using TaskManager.MVVM.ViewModels;
using TaskManager.MVVM.Models;
using TaskManager.Services;

namespace TaskManager.MVVM.View;

public partial class MainView : ContentPage
{
    private readonly MainViewModels _mainViewModels;

    public MainView(Database database)
    {
        InitializeComponent();
        _mainViewModels = new MainViewModels(database);
        BindingContext = _mainViewModels;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var taskView = new NewTaskView(_mainViewModels.Database, _mainViewModels.Categories, _mainViewModels.Tasks);
        await Navigation.PushAsync(taskView);
    }

    private async void DeleteTaskClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is TaskModel task)
        {
            bool confirm = await DisplayAlert("Delete Task", $"Are you sure you want to delete the task '{task.TaskName}'?", "Yes", "No");
            if (confirm)
                await _mainViewModels.DeleteTaskAsync(task);
        }
    }

    private async void EditTaskClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is TaskModel task)
        {
            string newTaskName = await DisplayPromptAsync("Edit Task", "Enter new task name:", initialValue: task.TaskName, maxLength: 50);
            if (!string.IsNullOrEmpty(newTaskName))
            {
                task.TaskName = newTaskName;
                await _mainViewModels.Database.UpdateTaskAsync(task);
                _mainViewModels.UpdateData();
            }
        }
    }

    private void CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        _mainViewModels.UpdateData();
    }
}
