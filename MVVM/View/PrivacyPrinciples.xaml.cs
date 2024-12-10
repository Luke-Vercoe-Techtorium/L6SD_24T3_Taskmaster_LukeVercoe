using TaskManager.Services;

namespace TaskManager.MVVM.View;

public partial class PrivacyPrinciples : ContentPage
{
    private readonly Database _database;

    public PrivacyPrinciples(Database database)
    {
        InitializeComponent();
        _database = database;
    }

    private async void OnAcceptClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainView(_database));
        Navigation.RemovePage(this);
    }

    private void OnDeclineClicked(object sender, EventArgs e)
    {
#if ANDROID
        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#endif
    }
}
