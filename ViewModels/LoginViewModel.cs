using Project_sfpgu_desktop.Models;
using Project_sfpgu_desktop.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace Project_sfpgu_desktop.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public ICommand CreateTestUserCommand => new RelayCommand(async _ =>
        {
            var user = await ApiService.CreateTestUserAsync();
            if (user != null)
            {
                MessageBox.Show($"Создан тестовый пользователь:\n{user.FullName} ({user.email})");
                Username = user.email;
                Password = user.password;
            }
        });

        public ICommand LoginCommand => new RelayCommand(async _ =>
        {
            MessageBox.Show($"Пытаемся войти как {Username} с паролем {Password}");

            var response = await ApiService.LoginAsync(Username, Password);
            if (response == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            TokenService.SaveToken(response.Token, response.ExpiresAt);

            App.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.NavigateToMainPage(response.User);
            });
        });

    }

}
