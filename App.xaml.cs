using TaskManager.MVVM.View;
using TaskManager.Services;

namespace TaskManager
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            MainPage = new NavigationPage(serviceProvider.GetRequiredService<PrivacyPrinciples>());
        }
    }
}
