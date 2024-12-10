using System.Collections.ObjectModel;
using TaskManager.MVVM.Models;
using TaskManager.MVVM.ViewModels;
using TaskManager.Services;

namespace TaskManager.MVVM.View;

public partial class NewTaskView : ContentPage
{
    private NewTaskViewModel _viewModel;

    public NewTaskView(Database database, ObservableCollection<CategoryModel> categories, ObservableCollection<TaskModel> tasks)
    {
        InitializeComponent();
        _viewModel = new(database, categories, tasks);
        BindingContext = _viewModel;
    }

    private async void AddTaskClicked(object sender, EventArgs e)
    {
        var selectedCategory = _viewModel.Categories.FirstOrDefault(x => x.IsSelected);

        if (selectedCategory is not null)
        {
            var task = new TaskModel
            {
                TaskName = _viewModel.Task,
                CategoryID = selectedCategory.Id,
            };
            await _viewModel.AddTaskAsync(task);
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Invalid Selection", "You must select a category", "OK");
        }
    }

    private async void AddCategoryClicked(object sender, EventArgs e)
    {
        string category = await DisplayPromptAsync("New Category", "Write the new Category Name", maxLength: 25, keyboard: Keyboard.Text);
        var random = new Random();

        if (!string.IsNullOrEmpty(category))
        {
            int newId = _viewModel.Categories.Any() ? _viewModel.Categories.Max(x => x.Id) + 1 : 1;
            var newCategory = new CategoryModel
            {
                Id = newId,
                Color = Color.FromRgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)).ToHex(),
                CategoryName = category,
            };
            await _viewModel.AddCategoryAsync(newCategory);
        }
    }

    private async void DeleteCategoryClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            int categoryId = (int)button.CommandParameter;
            var category = _viewModel.Categories.FirstOrDefault(c => c.Id == categoryId);

            if (category is not null)
            {
                bool confirm = await DisplayAlert("Delete Category", $"Are you sure you want to delete the category '{category.CategoryName}'?", "Yes", "No");

                if (confirm)
                    await _viewModel.DeleteCategoryAsync(category);
            }
        }
    }
}