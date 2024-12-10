using SQLite;
using PropertyChanged;

namespace TaskManager.MVVM.Models;

[AddINotifyPropertyChangedInterface]
public class CategoryModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    [Ignore]
    public int PendingTasks { get; set; }
    [Ignore]
    public int Completed { get; set; }
    [Ignore]
    public float Percentage { get; set; }
    [Ignore]
    public bool IsSelected { get; set; }
}
