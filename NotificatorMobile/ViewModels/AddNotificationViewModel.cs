using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using NotificatorMobile.Validation;
using FluentValidation.Results;

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
        private DateTime date = DateTime.Today;
        [ObservableProperty]
        private TimeSpan time;
        [ObservableProperty]
        private bool isRecurring;

        [ObservableProperty]
        private string? titleError;
        [ObservableProperty]
        public string? descriptionError;
        [ObservableProperty]
        public string? timeError;
        [ObservableProperty]
        private ValidationResult? validationResult;

        public ICommand ConfirmCommand { get; }

        public AddNotificationViewModel()
        {
            ConfirmCommand = new Command(async (object o) => await Confirm());
        }

        public async Task Confirm()
        {
            Debug.WriteLine($"{Title} {Description} {Date} {Time} {IsRecurring}");
            Debug.WriteLine($"Date {Date}");
            Debug.WriteLine($"Time {Time}");
            Debug.WriteLine($"Date + time {Date.Date + Time} vs now {DateTime.Now}");

            var validator = new AddNotificationPageValidator();
            ValidationResult = await validator.ValidateAsync(this);
            if (!ValidationResult.IsValid)
            {
                ApplyErrors();
                Debug.WriteLine($"Error - {TimeError}");
                Debug.WriteLine("VALIDATION FAILED");
                return;
            }
            Debug.WriteLine("VALIDATION SUCCESS");
            ClearErrors();          
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
