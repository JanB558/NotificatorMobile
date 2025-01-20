using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using NotificatorMobile.Services;
using NotificatorMobile.Models;
using System.Windows.Input;
using System.Diagnostics;

namespace NotificatorMobile.ViewModels
{
#pragma warning disable MVVMTK0032
#pragma warning disable MVVMTK0049
#pragma warning disable MVVMTK0045 //silence warnings that it won't work on windows, this is not windows app

    [INotifyPropertyChanged]
    public partial class MainViewModel
    {
        [ObservableProperty]
        private string _noContent = "There is nothing to show.";
        [ObservableProperty]
        private ICollection<Notification>? _notifications;

        //public ICommand InitializeCommand { get; }

        private readonly INotificationService _notificationService;
        public MainViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
            //InitializeCommand = new Command(async (object o) => await Initialize());
        }

        public async Task Initialize()
        {
            Notifications = (await _notificationService.GetAll() ?? Enumerable.Empty<Notification>())
                .OrderBy(notification => notification.TimeAndDate)
                .ToList();
            Debug.WriteLine($"Notifications - {Notifications.Count}");
        }

        public async Task Delete(int id)
        {
            await _notificationService.Delete(id);
        }
    }

#pragma warning restore
}
