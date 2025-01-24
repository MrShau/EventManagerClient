using EventsManager.API;
using EventsManager.Session;
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
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        private UserApi _userApi;

        public AddEmployeeWindow()
        {
            InitializeComponent();

            ComboBoxRole.IsEnabled = UserSession.Role == "ADMIN";
            _userApi = new UserApi();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = TextBoxLogin.Text;
            string firstName = TextBoxFirstName.Text;
            string lastName = TextBoxLastName.Text;
            string password =TextBoxPassword.Text;

            string role = "USER";

            if (ComboBoxRole.IsEnabled)
            {
                if (ComboBoxRole.SelectedIndex == 0)
                    role = "ADMIN";
                else if (ComboBoxRole.SelectedIndex == 1)
                    role = "MODER";
            }

            if (login.Length < 3)
            {
                MessageBox.Show("Логин слишком короткий", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (firstName.Length < 2)
            {
                MessageBox.Show("Введите имя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (lastName.Length < 2)
            {
                MessageBox.Show("Введите фамилию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password.Length < 7)
            {
                MessageBox.Show("Минимальная длина пароля: 7", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            await _userApi.SignUp(login, password, firstName, lastName, role);
        }
    }
}
