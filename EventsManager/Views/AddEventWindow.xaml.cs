using EventsManager.API;
using EventsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EventsManager.Views
{
    /// <summary>
    /// Логика взаимодействия для AddEventWindow.xaml
    /// </summary>
    public partial class AddEventWindow : Window
    {
        private EventApi _eventApi;

        public AddEventWindow()
        {
            InitializeComponent();

            _eventApi = new EventApi();
        }

        private async void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!DatePickerDate.SelectedDate.HasValue || !TimePickerTime.SelectedTime.HasValue || TextBoxTitle.Text.Length < 3 || TextBoxDescription.Text.Length < 10 || ComboBoxType.SelectedIndex < 0)
                {
                    MessageBox.Show("Заполните все поля !", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                DateTime dateTime = new DateTime(
                    DateOnly.FromDateTime(DatePickerDate.SelectedDate.Value),
                    TimeOnly.FromDateTime(TimePickerTime.SelectedTime.Value));



                if (await _eventApi.Add(TextBoxTitle.Text, TextBoxDescription.Text, dateTime, (EventTypes)ComboBoxType.SelectedIndex))
                {
                    MessageBox.Show("Мероприятие было успешно добавлено");
                    this.Close();
                }
                else MessageBox.Show("Ошибка сервера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
    }
}
