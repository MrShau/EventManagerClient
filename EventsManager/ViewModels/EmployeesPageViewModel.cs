using EventsManager.API;
using EventsManager.Models;
using EventsManager.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EventsManager.ViewModels
{
    class EmployeesPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private UserApi _userApi;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<UserModel> _users;
        public ObservableCollection<UserModel> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                }
            }
        }

        public ICommand DeleteCommand { get; }
        public ICommand ChangePasswordCommand { get; }

        public ICommand NextPageCommand { get; }
        public ICommand PreviosPageCommand { get; }

        public EmployeesPageViewModel()
        {
            _users = new ObservableCollection<UserModel>();

            _userApi = new UserApi();

            _currentPage = 1;

            DeleteCommand = new RelayCommand<UserModel>(Delete);
            ChangePasswordCommand = new RelayCommand<UserModel>(ChangePassword);
            NextPageCommand = new RelayCommand(NextPage);
            PreviosPageCommand = new RelayCommand(PreviosPage);
        }

        public async void Delete(UserModel model)
        {
            if (model != null && Users.Contains(model))
            {
                await _userApi.Delete(model.Id);
                await Load();
            }
        }

        public async void NextPage()
        {
            if (Users.Count > 0)
            {
                CurrentPage++;
                await Load();
            }
        }
        public async void PreviosPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await Load();
            }
        }

        private ChangePasswordWindow _changePasswordWindow;

        public void ChangePassword(UserModel model)
        {
            if (_changePasswordWindow == null)
                _changePasswordWindow = new ChangePasswordWindow(model.Id);

            _changePasswordWindow.ShowDialog();
        }

        public async Task Load()
        {
            Users = new ObservableCollection<UserModel>(await _userApi.GetPage(_currentPage, 24));
            if (Users.Count == 0 && _currentPage > 1)
                PreviosPage();
        }
    }
}
