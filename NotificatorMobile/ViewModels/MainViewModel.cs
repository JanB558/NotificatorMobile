using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using NotificatorMobile.Services;

namespace NotificatorMobile.ViewModels
{
#pragma warning disable MVVMTK0032
#pragma warning disable MVVMTK0049
#pragma warning disable MVVMTK0045 //silence warnings that it won't work on windows, this is not windows app

    [INotifyPropertyChanged]
    public partial class MainViewModel
    {
        [ObservableProperty]
        private string _labelText = "test";

        private readonly INotificationService _notificationService;
        public MainViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
    }

#pragma warning restore
}
