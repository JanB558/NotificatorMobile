using FluentValidation;
using NotificatorMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificatorMobile.Validation
{
    public class AddNotificationPageValidator : AbstractValidator<AddNotificationViewModel>
    {
        public static string Title => nameof(AddNotificationViewModel.Title);
        public static string Description => nameof(AddNotificationViewModel.Description);

        public AddNotificationPageValidator() 
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().MinimumLength(3).MaximumLength(30);
            RuleFor(x => x.Description).NotEmpty().NotNull().MinimumLength(3).MaximumLength(300);
        }
    }
}
