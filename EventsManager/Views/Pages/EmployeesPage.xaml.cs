using EventsManager.API;
using EventsManager.Models;
using EventsManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        private EmployeesPageViewModel _viewModel;

        private UserApi _userApi;

        private AddEmployeeWindow _addEmployeeWindow;

        private bool _isEditing = false;

        public EmployeesPage()
        {
            InitializeComponent();

            _viewModel = (EmployeesPageViewModel)DataContext;
            
            _userApi = new UserApi();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.Load();
        }

        private async void ButtonAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (_addEmployeeWindow == null)
                _addEmployeeWindow = new AddEmployeeWindow();
            
            _addEmployeeWindow.ShowDialog();
            await _viewModel.Load();
            _addEmployeeWindow = null;
        }

        private async void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            await _userApi.Export();
        }

        private void DataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Row.Item is UserModel model)
            {
                model.Original = new UserModel
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = model.Role,
                    Login = model.Login
                };
            }
        }

        private async void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (_isEditing)
                    return;
                _isEditing = true;
                if (e.EditAction == DataGridEditAction.Commit)
                {
                    DataGrid dataGrid = sender as DataGrid;
                    dataGrid.CommitEdit(DataGridEditingUnit.Row, true);

                    if (e.Row.Item is UserModel model)
                    {
                        if (model.HasChanged())
                        {
                            await _userApi.Update(model);
                            await _viewModel.Load();
                        }
                        
                    }
                }
            }
            catch (Exception ex) { }
            finally { _isEditing = false; }
        }
    }
}
