using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using NotificatorMobile.Validation;
using FluentValidation.Results;
using NotificatorMobile.Services;
using CommunityToolkit.Mvvm.Messaging;
using NotificatorMobile.Utilities;

namespace NotificatorMobile.ViewModels
{
#pragma warning disable MVVMTK0032
#pragma warning disable MVVMTK0049
#pragma warning disable MVVMTK0045 //silence warnings that it won't work on windows, this is not windows app

    [INotifyPropertyChanged]
    public partial class AddNotificationViewModel
    {
        [ObservableProperty]
        private string _title = string.Empty;
        [ObservableProperty]
        private string _description = string.Empty;
        [ObservableProperty]
        private DateTime _date = DateTime.Today;
        [ObservableProperty]
        private TimeSpan _time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
        [ObservableProperty]
        private bool _isRecurring;

        [ObservableProperty]
        private string _message = "Create new notification";

        [ObservableProperty]
        private string? _titleError;
        [ObservableProperty]
        public string? _descriptionError;
        [ObservableProperty]
        public string? _timeError;
        [ObservableProperty]
        private ValidationResult? _validationResult;

        public bool IsUpdate { get; set; } = false;
        public int IdForUpdate { get; set; }

        public ICommand ConfirmCommand { get; }

        private readonly INotificationService _notificationService;

        public AddNotificationViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
            ConfirmCommand = new Command(async (object o) => await Confirm());
        }

        public async Task Confirm()
        {
            var validator = new AddNotificationPageValidator();
            ValidationResult = await validator.ValidateAsync(this);

            if (!ValidationResult.IsValid) // failed - abort
            {
                ApplyErrors();
                Debug.WriteLine("VALIDATION FAILED");
                return;
            }

            Debug.WriteLine("VALIDATION SUCCESS");
            ClearErrors();

            //success
            if (IsUpdate == false)
            {
                var notification = new Models.Notification
                {
                    Title = Title,
                    Description = Description,
                    TimeAndDate = Date.Date.Add(Time),
                    IsRecurring = IsRecurring
                };
                await _notificationService.Create(notification);
                await _notificationService.Register(notification);
            }
            else
            {
                var notification = new Models.Notification
                {
                    Id = IdForUpdate,
                    Title = Title,
                    Description = Description,
                    TimeAndDate = Date.Date.Add(Time),
                    IsRecurring = IsRecurring,
                };
                await _notificationService.Update(notification);
                _notificationService.Cancel(IdForUpdate);
                await _notificationService.Register(notification);
            }
            //go back to page
            WeakReferenceMessenger.Default.Send(new NavigateBackMessage(true));          
        }

        private void ApplyErrors()
        {
            TitleError = ValidationResult?.Errors.FirstOrDefault(x => x.PropertyName == nameof(Title))?.ErrorMessage;
            DescriptionError = ValidationResult?.Errors.FirstOrDefault(x => x.PropertyName == nameof(Description))?.ErrorMessage;
            TimeError = ValidationResult?.Errors.FirstOrDefault(x => x.PropertyName == nameof(Time))?.ErrorMessage;
        }

        private void ClearErrors()
        {
            TitleError = null;
            DescriptionError = null;
            TimeError = null;
        }
    }

#pragma warning restore
}
