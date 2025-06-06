using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Project_sfpgu_desktop.Models;

namespace Project_sfpgu_desktop.Views
{
    public partial class RegisterUserPage : Page
    {
        private readonly string _jwtToken;

        public RegisterUserPage(string jwtToken)
        {
            InitializeComponent();
            _jwtToken = jwtToken;

            RoleComboBox.SelectedIndex = 0;
            RoleComboBox_SelectionChanged(RoleComboBox, null);
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new MainPage(new UserModel
            {
                FullName = "Admin",
                Role = "admin"
            });
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupLabel == null || GroupTextBox == null)
            {
                MessageBox.Show("GroupLabel или GroupTextBox не найдены. Проверь XAML и InitializeComponent.");
                return;
            }

            var role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();

            if (role == "student")
            {
                GroupLabel.Visibility = Visibility.Visible;
                GroupTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                GroupLabel.Visibility = Visibility.Collapsed;
                GroupTextBox.Visibility = Visibility.Collapsed;
                GroupTextBox.Text = "";
            }
        }


        private async void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            var fullName = FullNameTextBox.Text.Trim();
            var email = EmailTextBox.Text.Trim();
            var password = PasswordBox.Password.Trim();
            var role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            var group = GroupTextBox.Text.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (role == "student" && string.IsNullOrEmpty(group))
            {
                MessageBox.Show("Пожалуйста, укажите группу для студента.");
                return;
            }

            var newUser = new
            {
                FullName = fullName,
                Email = email,
                Password = password,
                Role = role,
                Group = role == "student" ? group : null
            };

            var json = JsonSerializer.Serialize(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

                var response = await httpClient.PostAsync("https://localhost:25565/auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Пользователь успешно создан!");
                    FullNameTextBox.Text = "";
                    EmailTextBox.Text = "";
                    PasswordBox.Password = "";
                    RoleComboBox.SelectedIndex = 0;
                    GroupTextBox.Text = "";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ошибка при создании пользователя:\n{error}");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка запроса: {ex.Message}");
            }
        }
    }
}
