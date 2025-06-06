using System.Windows;
using Project_sfpgu_desktop.Models;
using Project_sfpgu_desktop.Views;

namespace Project_sfpgu_desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new LoginPage());
        }

        public void NavigateToMainPage(UserModel user)
        {
            MainFrame.Navigate(new MainPage(user));
        }
    }
}
