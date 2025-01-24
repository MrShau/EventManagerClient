using EventsManager.API;
using EventsManager.Models;
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
    class EventsPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EventModel> _events;
        public ObservableCollection<EventModel> Events
        {
            get => _events;
            set
            {
                _events = value;
                OnPropertyChanged(nameof(Events));
            }
        }

        private EventApi _eventApi;
        public bool IncludeCompleted = true;

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
        public ICommand AddParticipantCommand { get; }

        public ICommand NextPageCommand { get; }
        public ICommand PreviosPageCommand { get; }

        public EventsPageViewModel()
        {
            _currentPage = 1;
            _eventApi = new EventApi();
            _events = new ObservableCollection<EventModel>();

            DeleteCommand = new RelayCommand<EventModel>(Delete);
            AddParticipantCommand = new RelayCommand<EventModel>(AddParticipant);
            NextPageCommand = new RelayCommand(NextPage);
            PreviosPageCommand = new RelayCommand(PreviosPage);
        }

        public async void Delete(EventModel model)
        {
            if (model != null && Events.Contains(model))
            {
                await _eventApi.Delete(model.Id);
                await Load();
            }
        }

        public async void AddParticipant(EventModel model)
        {
            await _eventApi.AddParticipant(model);
            await Load();
        }

        public async Task Load()
        {
            Events = new ObservableCollection<EventModel>(await _eventApi.GetPage(_currentPage, 8, IncludeCompleted));
            if (Events.Count == 0 && _currentPage > 1)
                PreviosPage();
        }

        public async void NextPage()
        {
            if (Events.Count > 0)
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
