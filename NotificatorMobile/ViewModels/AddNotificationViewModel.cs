using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;

namespace NotificatorMobile.ViewModels
{
#pragma warning disable MVVMTK0032
#pragma warning disable MVVMTK0049
#pragma warning disable MVVMTK0045 //silence warnings that it won't work on windows, this is not windows app

    public partial class AddNotificationViewModel : ObservableValidator
    {
        [ObservableProperty]
        [Required(ErrorMessage = "Title is required.")]
        [MinLength(3, ErrorMessage = "Title must be at least {1} characters long.")]
        [MaxLength(30, ErrorMessage = "Title cannot be longer than {1} characters.")]
        private string title = string.Empty;
        [ObservableProperty]
        [Required(ErrorMessage ="Description is required.")]
        [MinLength(3, ErrorMessage = "Description must be at least {1} characters long.")]
        [MaxLength(300, ErrorMessage = "Description cannot be longer than {1} characters.")]
        private string description = string.Empty;
        [ObservableProperty]
        private DateTime date;
        [ObservableProperty]
        private TimeSpan time;
        [ObservableProperty]
        private bool isRecurring;

        public string? TitleError => GetErrors(nameof(Title))?.Cast<ValidationResult>().FirstOrDefault()?.ErrorMessage;
        public string? DescriptionError => GetErrors(nameof(Description))?.Cast<ValidationResult>().FirstOrDefault()?.ErrorMessage;
        public string? TimeError => GetErrors(nameof(Time))?.Cast<ValidationResult>().FirstOrDefault()?.ErrorMessage;

        public ICommand ConfirmCommand { get; }

        public AddNotificationViewModel()
        {
            ConfirmCommand = new Command(Confirm);
        }

        public void Confirm()
        {
            ValidateAllProperties();
            OnPropertyChanged(nameof(TitleError)); 
            OnPropertyChanged(nameof(DescriptionError)); 
            OnPropertyChanged(nameof(TimeError));

            if (HasErrors) Debug.WriteLine("Validation Failed"); else Debug.WriteLine("Validation Success");
            Debug.WriteLine($"{Title} {Description} {Date} {Time} {IsRecurring}");
        }
    }

#pragma warning restore
}
