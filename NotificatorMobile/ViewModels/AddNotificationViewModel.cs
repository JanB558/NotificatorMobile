using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace NotificatorMobile.ViewModels
{
#pragma warning disable MVVMTK0032
#pragma warning disable MVVMTK0049
#pragma warning disable MVVMTK0045 //silence warnings that it won't work on windows, this is not windows app

    [INotifyPropertyChanged]
    public partial class AddNotificationViewModel
    {
        [ObservableProperty]
        private string title = string.Empty;
        [ObservableProperty]
        private string description = string.Empty;
        [ObservableProperty]
        private DateTime date;
        [ObservableProperty]
        private DateTime hour;
        [ObservableProperty]
        private bool isRecurring;

        public ICommand ConfirmCommand { get; }

        public AddNotificationViewModel()
        {
            ConfirmCommand = new Command(Confirm);
        }

        public void Confirm()
        {
            Debug.WriteLine("Button clicked");
            Debug.WriteLine($"{Title} {Description} {Date} {Hour} {IsRecurring}");
        }
    }

#pragma warning restore
}
