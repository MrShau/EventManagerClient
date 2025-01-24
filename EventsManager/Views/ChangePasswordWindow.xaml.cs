using EventsManager.API;
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
    /// Логика взаимодействия для ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private UserApi _userApi;
        private int _id;

        public ChangePasswordWindow(int id)
        {
            InitializeComponent();

            _userApi = new UserApi();
            _id = id;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string password = TextBoxPassword.Text;
            if (password.Length < 7)
            {
                MessageBox.Show("Минимальная длина пароля: 7", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (await _userApi.ChangePassword(_id, password))
                this.Close();
        }
    }
}
