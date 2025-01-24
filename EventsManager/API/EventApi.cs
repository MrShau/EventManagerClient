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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace EventsManager.API
{
    class EventApi
    {
        private HttpClient _httpClient;

        public EventApi()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserSession.JWT_TOKEN);
        }

        public async Task<bool> Add(string title, string description, DateTime dateTime, EventTypes eventType)
        {
            try
            {
                HttpResponseMessage resposne = await _httpClient.PostAsJsonAsync($"{Consts.API_BASE}/event/add", new
                {
                    Title = title,
                    Description = description,
                    DateTime = dateTime,
                    EventType = eventType
                });

                return resposne.IsSuccessStatusCode;
            }
            catch (Exception ex) { }
            return false;
        }

        public async Task<List<EventModel>> GetPage(int page, int count, bool includeCompleted = true)
        {
            List<EventModel> list = new List<EventModel>();
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{Consts.API_BASE}/event/get_page?page={page}&count={count}&includeCompleted={includeCompleted}");

                if (response.IsSuccessStatusCode)
                {
                    list = JsonConvert.DeserializeObject<List<EventModel>>(await response.Content.ReadAsStringAsync());
                    foreach (var item in list)
                    {
                        item.Date = item.DateTime;
                        item.Time = item.DateTime;
                    }
                }



            }
            catch (Exception ex) { MessageBox.Show("Ошибка загрузки мероприятий", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
            return list;
        }

        public async Task Update(EventModel model)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{Consts.API_BASE}/event/update", new
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    DateTime = new DateTime(DateOnly.FromDateTime(model.Date), TimeOnly.FromDateTime(model.Time)),
                    EventType = model.EventType
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

        public async Task AddParticipant(EventModel model)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserSession.JWT_TOKEN);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{Consts.API_BASE}/event/add_participant", new { Id = model.Id });

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Вы добавлены на мероприятие");
                    return;
                }

            }
            catch (Exception ex) { }

            MessageBox.Show("Ошибка сервера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public async Task Delete(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{Consts.API_BASE}/event/delete", new
                {
                    Id = id,
                });

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Мероприятие удалено");
                    return;
                }

            }
            catch (Exception ex) { }

            MessageBox.Show("Ошибка сервера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public async Task Export()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{Consts.API_BASE}/event/export");
                if (response.IsSuccessStatusCode)
                {
                    var file = await response.Content.ReadAsByteArrayAsync();

                    var saveFileDialog = new SaveFileDialog()
                    {
                        Filter = "Excel Files|*.xlsx",
                        FileName = "Мероприятия.xlsx"
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
    }
}
