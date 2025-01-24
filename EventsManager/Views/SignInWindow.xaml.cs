using EventsManager.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
    /// Логика взаимодействия для SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        private readonly UserApi _userApi;

        public SignInWindow()
        {
            InitializeComponent();

            _userApi = new UserApi();
        }

        private async void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
                button.IsEnabled = false;

            if (await _userApi.SignIn(TextBoxLogin.Text, PasswordBoxUser.Password))
            {
                await _userApi.Auth();
                
                this.Close();
            }
            else
            {
                if (button != null)
                    button.IsEnabled = true;
            }

        }
    }
}
