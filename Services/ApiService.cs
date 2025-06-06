using Project_sfpgu_desktop.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace Project_sfpgu_desktop.Services
{
    public static class ApiService
    {
        private static readonly HttpClient _http;

        static ApiService()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            _http = new HttpClient(handler);
            _http.BaseAddress = new Uri("https://localhost:25565/");
        }



        public static async Task<LoginResponse> LoginAsync(string email, string password)
        {
            var request = new LoginRequest { email = email, password = password };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:25565/auth/login");
            httpRequest.Content = content;

            var response = await _http.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка авторизации: {response.StatusCode}\n{errorContent}");
                return null;
            }


            var responseBody = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return loginResponse;
        }

        public static async Task<List<UserModel>> GetStudentsByGroupAsync(string groupName)
        {
            string encodedGroupName = Uri.EscapeDataString(groupName);
            return await GetAsync<List<UserModel>>($"api/students/group/{encodedGroupName}");
        }


        public static async Task<List<AttendanceRecord>> GetAttendanceByScheduleIdAsync(string scheduleId)
        {
            var response = await _http.GetAsync($"api/schedule/attendance/schedule/{scheduleId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<AttendanceRecord>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            return null;
        }


        public static async Task<UserModel> CreateTestUserAsync()
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:25565/auth/dev-create-user");

            var response = await _http.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка создания пользователя: {response.StatusCode}\n{errorContent}");
                return null;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserModel>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            MessageBox.Show($"Тестовый пользователь создан:\nEmail: {user.email}\nПароль: {user.password}");
            return user;
        }

        public static async Task<List<ScheduleItem>> GetScheduleAsync(string groupName)
        {
            var response = await _http.GetAsync($"https://localhost:25565/api/schedule/group/{groupName}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка загрузки расписания: {response.StatusCode}\n{errorContent}");
                return null;
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ScheduleItem>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static async Task<List<AttendanceRecord>> GetAttendanceAsync(string groupName)
        {
            var response = await _http.GetAsync($"https://localhost:25565/api/schedule/attendance/group/{groupName}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка загрузки посещаемости: {response.StatusCode}\n{errorContent}");
                return null;
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<AttendanceRecord>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static async Task<bool> UpdateAttendanceAsync(AttendanceRecord record)
        {
            var json = JsonSerializer.Serialize(record);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("https://localhost:25565/api/schedule/attendance/update", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка обновления посещаемости: {response.StatusCode}\n{errorContent}");
                return false;
            }
            return true;
        }
        public static void ClearAuthHeader()
        {
            if (_http.DefaultRequestHeaders.Contains("Authorization"))
                _http.DefaultRequestHeaders.Remove("Authorization");
        }
        public static async Task<bool> DeleteScheduleItemAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"https://localhost:25565/api/schedule/delete/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка удаления записи расписания: {response.StatusCode}\n{errorContent}");
                return false;
            }
            return true;
        }

        public static async Task<bool> AddScheduleItemAsync(ScheduleItem item)
        {
            var json = JsonSerializer.Serialize(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("https://localhost:25565/api/schedule/add", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка добавления записи: {response.StatusCode}\n{errorContent}");
                return false;
            }
            return true;
        }

        public static async Task<bool> UpdateScheduleItemAsync(ScheduleItem item)
        {
            var json = JsonSerializer.Serialize(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PutAsync($"https://localhost:25565/api/schedule/update/{item.Id}", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка обновления записи: {response.StatusCode}\n{errorContent}");
                return false;
            }
            return true;
        }

        private static async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _http.GetAsync($"https://localhost:25565/{endpoint}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка запроса: {response.StatusCode}\n{errorContent}");
                return default;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public static async Task<UserModel> AddStudentAsync(UserModel newStudent)
        {
            var json = JsonSerializer.Serialize(newStudent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("https://localhost:25565/api/students/add", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка добавления студента: {response.StatusCode}\n{errorContent}");
                return null;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var addedStudent = JsonSerializer.Deserialize<UserModel>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return addedStudent;
        }

        public static void AddAuthHeader()
        {
            if (TokenService.IsTokenValid())
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenService.Token);
            }
        }
    }
}
