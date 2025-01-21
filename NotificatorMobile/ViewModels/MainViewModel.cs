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
#pragma warning disable MVVMTK0032 //this is some weird issue that many other people encounter in one way or another, 
    //but code compiles and works with no issues
#pragma warning disable MVVMTK0045 //silence warnings that it won't work on windows, this is not windows app

    [INotifyPropertyChanged]
    public partial class MainViewModel
    {
        [ObservableProperty]
        private string _noContent = "There is nothing to show.";
        [ObservableProperty]
        private ICollection<Notification>? _notifications;

        private readonly INotificationService _notificationService;
        public MainViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Initialize()
        {
            Notifications = (await _notificationService.GetAll() ?? Enumerable.Empty<Notification>())
                .OrderBy(notification => notification.TimeAndDate)
                .ToList();
        }

        public async Task Delete(int id)
        {
            await _notificationService.Delete(id);
            _notificationService.Cancel(id);
        }

        public INotificationService GetServiceHandle()
        {
            return _notificationService;
        }
    }
#pragma warning restore
}
