using System.Windows;
using System.Windows.Controls;
using Project_sfpgu_desktop.Models;

namespace Project_sfpgu_desktop.Views
{
    public partial class AttendancePage : Page
    {
        private readonly UserModel _user;

        public AttendancePage(UserModel user)
        {
            InitializeComponent();
            _user = user;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new MainPage(_user);
        }
    }
}
