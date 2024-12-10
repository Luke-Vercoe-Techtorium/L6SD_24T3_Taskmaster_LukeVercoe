using TaskManager.MVVM.View;

namespace TaskManager;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        MainPage = new NavigationPage(serviceProvider.GetRequiredService<PrivacyPrinciples>());
    }
}
