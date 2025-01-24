using EventsManager.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EventsManager.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : Page
    {
        private EventsPageViewModel _viewModel;

        public CalendarPage()
        {
            InitializeComponent();

            _viewModel = (EventsPageViewModel)DataContext;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.IncludeCompleted = false;
            await _viewModel.Load();
        }
    }
}
