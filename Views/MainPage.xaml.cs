using System.Windows;
using System.Windows.Controls;
using Project_sfpgu_desktop.Models;
using Project_sfpgu_desktop.Services;
using Project_sfpgu_desktop.Views;

namespace Project_sfpgu_desktop.Views
{
    public partial class MainPage : Page
    {
        private UserModel _user;

        public MainPage(UserModel user)
        {
            InitializeComponent();
            _user = user;
            WelcomeText.Text = $"Добро пожаловать, {_user.FullName} ({_user.Role})";

            if (_user.Role == "admin")
            {
                CreateUserButton.Visibility = Visibility.Visible;
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            TokenService.ClearToken();
            ApiService.ClearAuthHeader();
            Application.Current.MainWindow.Content = new LoginPage();
        }



        private void OpenSchedule_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SchedulePage(_user.Role);
        }


        private void OpenRegisterUser_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new RegisterUserPage(TokenService.Token);
        }


        private void OpenAttendance_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new AttendancePage(_user);
        }

    }
}
