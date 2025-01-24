using EventsManager.API;
using EventsManager.Models;
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
    /// Логика взаимодействия для EventsPage.xaml
    /// </summary>
    public partial class EventsPage : Page
    {
        private EventsPageViewModel _viewModel;

        private AddEventWindow _addEventWindow;

        private string Title;
        private string Description;
        private DateOnly Date;
        private TimeOnly Time;

        private EventApi _eventApi;

        private bool _isEditing = false;

        public EventsPage()
        {
            InitializeComponent();

            _eventApi = new EventApi();

            _viewModel = (EventsPageViewModel)DataContext;
        }

        private async void ButtonAddEvent_Click(object sender, RoutedEventArgs e)
        {
            if (_addEventWindow == null)
                _addEventWindow = new AddEventWindow();

            _addEventWindow.ShowDialog();
            _addEventWindow.Close();
            _addEventWindow = null;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.Load();
        }

        private void DataGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MessageBox.Show(e.NewValue.ToString());
        }

        private async void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (_isEditing)
                    return;
                _isEditing = true;

                DataGridEvents.IsEnabled = false;

                if (e.EditAction == DataGridEditAction.Commit)
                {
                    DataGrid dataGrid = sender as DataGrid;
                    dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                    
                    if (e.Row.Item is EventModel eventModel )
                    {
                        if (eventModel.HasChanged())
                        {
                            await _eventApi.Update(eventModel);
                            await _viewModel.Load();
                        }
                    }
                }
            }
            catch (Exception ex) { }
            finally { _isEditing = false; DataGridEvents.IsEnabled = true; }
            
        }

        private void DataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Row.Item is EventModel model)
            {
                model.Original = new EventModel
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    DateTime = model.DateTime,
                    EventType = model.EventType,
                    Date = model.Date,
                    Time = model.Time
                };
            }
        }

        private async void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            await _eventApi.Export();
        }
    }
}
