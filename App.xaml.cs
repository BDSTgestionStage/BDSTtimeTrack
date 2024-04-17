namespace TimeTrack
{
    public partial class App : Application
    {
        public App()
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
            InitializeComponent();

            MainPage = new AppShell();

        }
    }
}
