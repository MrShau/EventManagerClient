using EventsManager.Models;
using EventsManager.Session;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EventsManager.API
{
    internal class UserApi
    {
        private HttpClient _httpClient;

        public UserApi()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> SignIn(string login, string password)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{Consts.API_BASE}/user/signin", new
                {
                    Login = login,
                    Password = password
                });

                if (response.IsSuccessStatusCode)
                {
                    UserSession.JWT_TOKEN = await response.Content.ReadAsStringAsync();
                    return true;
                }

                MessageBox.Show(await response.Content.ReadAsStringAsync(), "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка, пожалуйста попробуйте позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
        }

        public async Task<bool> SignUp(string login, string password, string firstName, string lastName, string role)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserSession.JWT_TOKEN);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{Consts.API_BASE}/user/signup", new
                {
                    Login = login,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    Role = role
                });

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Пользователь успешно добавлен", "Пользователь добавлен", MessageBoxButton.OK);
                    return true;
                }

                MessageBox.Show(await response.Content.ReadAsStringAsync(), "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка, пожалуйста попробуйте позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return false;
        }

        public async Task Auth()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserSession.JWT_TOKEN);
                HttpResponseMessage response = await _httpClient.GetAsync($"{Consts.API_BASE}/user/auth");

                if (response.IsSuccessStatusCode)
                {
                    UserModel res = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync());
                    if (res != null)
                    {
                        UserSession.Login = res.Login;
                        UserSession.FirstName = res.FirstName;
                        UserSession.LastName = res.LastName;
                        UserSession.Role = res.Role;
                    }
                    return;
                }

                MessageBox.Show(await response.Content.ReadAsStringAsync(), "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка, пожалуйста попробуйте позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task<List<UserModel>> GetPage(int page, int count)
        {
            List<UserModel> list = new List<UserModel>();
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserSession.JWT_TOKEN);
                HttpResponseMessage response = await _httpClient.GetAsync($"{Consts.API_BASE}/user/get?page={page}&count={count}");

                if (response.IsSuccessStatusCode)
                {
                    list = JsonConvert.DeserializeObject<List<UserModel>>(await response.Content.ReadAsStringAsync());
                    return list;
                }

                MessageBox.Show(await response.Content.ReadAsStringAsync(), "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка, пожалуйста попробуйте позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return list;
        }

        public async Task Delete(int id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserSession.JWT_TOKEN);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{Consts.API_BASE}/user/delete", new { Id = id });

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Пользователь успешно удален");
                    return;
                }

                MessageBox.Show(await response.Content.ReadAsStringAsync(), "Ошибка сервера", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка, пожалуйста попробуйте позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task Update(UserModel model)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserSession.JWT_TOKEN);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{Consts.API_BASE}/user/update", new
                {
                    Id = model.Id,
                    Login = model.Login,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = model.Role,
                });

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Изменения применены");
                    return;
                }

            }
            catch (Exception ex) { }

            MessageBox.Show("Ошибка сервера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public async Task<bool> ChangePassword(int id, string password)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserSession.JWT_TOKEN);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{Consts.API_BASE}/user/change_password", new
                {
                    Id = id,
                    Password = password
                });

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Изменения применены");
                    return true;
                }

            }
            catch (Exception ex) { }

            MessageBox.Show("Ошибка сервера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        public async Task Export()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{Consts.API_BASE}/user/export");
                if (response.IsSuccessStatusCode)
                {
                    var file = await response.Content.ReadAsByteArrayAsync();

                    var saveFileDialog = new SaveFileDialog()
                    {
                        Filter = "Excel Files|*.xlsx",
                        FileName = "Сотрудники.xlsx"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        await File.WriteAllBytesAsync(saveFileDialog.FileName, file);
                        MessageBox.Show("Файл успешно сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else MessageBox.Show("Ошибка при сохранении файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }
            }
            catch (Exception ex) { }

            MessageBox.Show("Ошибка сервера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        ~UserApi()
        {
            _httpClient.Dispose();
        }
    }
}
