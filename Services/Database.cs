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
        await _database.CreateTableAsync<CategoryModel>();
        await _database.CreateTableAsync<TaskModel>();
    }

    public async Task<List<CategoryModel>> GetCategoriesAsync() => await _database.Table<CategoryModel>().ToListAsync();
    public async Task<int> SaveCategoryAsync(CategoryModel category) => await _database.InsertAsync(category);
    public async Task<int> UpdateCategoryAsync(CategoryModel category) => await _database.UpdateAsync(category);
    public async Task<int> DeleteCategoryAsync(CategoryModel category) => await _database.DeleteAsync(category);

    public async Task<List<TaskModel>> GetTasksAsync() => await _database.Table<TaskModel>().ToListAsync();
    public async Task<int> SaveTaskAsync(TaskModel task) => await _database.InsertAsync(task);
    public async Task<int> UpdateTaskAsync(TaskModel task) => await _database.UpdateAsync(task);
    public async Task<int> DeleteTaskAsync(TaskModel task) => await _database.DeleteAsync(task);
}
