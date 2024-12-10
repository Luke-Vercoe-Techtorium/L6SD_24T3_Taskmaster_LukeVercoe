using SQLite;
using TaskManager.MVVM.Models;

namespace TaskManager.Services;

public class Database
{
    private readonly SQLiteAsyncConnection _database;

    public Database()
    {
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "TaskManager.db");
        _database = new(databasePath);

        InitializeTables();
    }

    private async void InitializeTables()
    {
        await Task.WhenAll(
            _database.CreateTableAsync<CategoryModel>(),
            _database.CreateTableAsync<TaskModel>()
        );
    }

    public Task<List<CategoryModel>> GetCategoriesAsync() => _database.Table<CategoryModel>().ToListAsync();
    public Task<int> SaveCategoryAsync(CategoryModel category) => _database.InsertAsync(category);
    public Task<int> UpdateCategoryAsync(CategoryModel category) => _database.UpdateAsync(category);
    public Task<int> DeleteCategoryAsync(CategoryModel category) => _database.DeleteAsync(category);

    public Task<List<TaskModel>> GetTasksAsync() => _database.Table<TaskModel>().ToListAsync();
    public Task<int> SaveTaskAsync(TaskModel task) => _database.InsertAsync(task);
    public Task<int> UpdateTaskAsync(TaskModel task) => _database.UpdateAsync(task);
    public Task<int> DeleteTaskAsync(TaskModel task) => _database.DeleteAsync(task);
}
