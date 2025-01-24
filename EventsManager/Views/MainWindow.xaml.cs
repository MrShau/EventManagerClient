using EventsManager.API;
using EventsManager.Session;
using EventsManager.Views;
using EventsManager.Views.Pages;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EventsManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SignInWindow _signInWindow;

        private CalendarPage _calendarPage;
        private EventsPage _eventsPage;
        private EmployeesPage _employeesPage;

        private UserApi _userApi;

        public string FirstName;

        public MainWindow()
        {
            InitializeComponent();

            _signInWindow = new SignInWindow();
            _signInWindow.ShowDialog();

            _userApi = new UserApi();

            if (UserSession.JWT_TOKEN == null)
            {
                this.Close();
            }

            FirstName = UserSession.FirstName;
        }

        private void ChangeSelectedButton(Button button)
        {
            ButtonCalendarPage.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            ButtonEventsPage.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            ButtonEmployeesPage.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

            button.Background = new SolidColorBrush(Color.FromRgb(96, 125, 139));

        }

        private void ButtonEventsPage_Click(object sender, RoutedEventArgs e)
        {
            if (_eventsPage == null)
                _eventsPage = new EventsPage();
            FramePage.Content = _eventsPage;
            ChangeSelectedButton(ButtonEventsPage);
        }

        private void ButtonCalendarPage_Click(object sender, RoutedEventArgs e)
        {
            if (_calendarPage == null)
                _calendarPage = new CalendarPage();
            FramePage.Content = _calendarPage;
            ChangeSelectedButton(ButtonCalendarPage);
        }

        private void ButtonEmployeesPage_Click(object sender, RoutedEventArgs e)
        {
            if (_employeesPage == null)
                _employeesPage = new EmployeesPage();
            FramePage.Content = _employeesPage;

            ChangeSelectedButton(ButtonEmployeesPage);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlockUserName.Text = $"{UserSession.FirstName} {UserSession.LastName}";
            TextBlockLogin.Text = $"#{UserSession.Login}";
            TextBlockRole.Text = $"{UserSession.Role}";

            ButtonEventsPage.IsEnabled = (UserSession.Role?.Equals("ADMIN") ?? false) || (UserSession.Role?.Equals("MODER") ?? false);
            ButtonEmployeesPage.IsEnabled = (UserSession.Role?.Equals("ADMIN") ?? false) || (UserSession.Role?.Equals("MODER") ?? false);

            if (_calendarPage == null)
                _calendarPage = new CalendarPage();
            FramePage.Content = _calendarPage; 

        }

        private void ButtonSignOut_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Process.GetCurrentProcess()?.MainModule?.FileName);
            Application.Current.Shutdown();
        }
    }
}