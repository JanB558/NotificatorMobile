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
        private DateTime date;
        [ObservableProperty]
        private TimeSpan time;
        [ObservableProperty]
        private bool isRecurring;

        //public string? TitleError => GetErrors(nameof(Title))?.Cast<ValidationResult>().FirstOrDefault()?.ErrorMessage;
        //public string? DescriptionError => GetErrors(nameof(Description))?.Cast<ValidationResult>().FirstOrDefault()?.ErrorMessage;
        //public string? TimeError => GetErrors(nameof(Time))?.Cast<ValidationResult>().FirstOrDefault()?.ErrorMessage;
        [ObservableProperty]
        private ValidationResult? validationResult;

        public ICommand ConfirmCommand { get; }

        public AddNotificationViewModel()
        {
            ConfirmCommand = new Command(async (object o) => await Confirm());
        }

        public async Task Confirm()
        {
            var validator = new AddNotificationPageValidator();
            ValidationResult = await validator.ValidateAsync(this);
            if (!ValidationResult.IsValid)
            {
                Debug.WriteLine("VALIDATION FAILED");
                return;
            }
            Debug.WriteLine("VALIDATION SUCCESS");
            Debug.WriteLine($"{Title} {Description} {Date} {Time} {IsRecurring}");
        }
    }

#pragma warning restore
}
