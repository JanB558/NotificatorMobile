using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificatorMobile.Converters
{
    internal class ShowErrorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType,
                           object? parameter, CultureInfo culture)
        {
            if (value is not ValidationResult validationResult ||
                                              validationResult.Errors.Count == 0)
            {
                return null;
            }

            if (parameter == null)
            {
                return null;
            }

            var property = parameter as string;
            return validationResult.Errors
              .FirstOrDefault(x => x.PropertyName.Split(".").LastOrDefault() == property)?
              .ErrorMessage;
        }

        public object? ConvertBack(object? value, Type targetType,
                                   object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException(
              "ConvertBack not implemented for the converter.");
        }
    }
}
