using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Markup.LeftToRight;
using NotificatorMobile.Converters;
using NotificatorMobile.ViewModels;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace NotificatorMobile.Pages;

public partial class AddNotificationPage : ContentPage
{
    private readonly AddNotificationViewModel viewModel = new();
    public AddNotificationPage()
	{
        BindingContext = viewModel;

        //declarations
        var errorConverter = new ShowErrorConverter();
        var buttonConfirm = new Button();
        buttonConfirm.CornerRadius = 50;
        var datePicker = new DatePicker();
        datePicker.MinimumDate = DateTime.Today;

        //content
        Content = new StackLayout
        {
            Children = 
            {
                new Label().Text("Title:").CenterVertical().FontSize(18),
                new Entry().CenterVertical().FontSize(14)
                    .Bind(Entry.TextProperty, nameof(viewModel.Title)),
                new Label().CenterVertical().FontSize(14).TextColor(Microsoft.Maui.Graphics.Colors.Red)
                    .Bind(Label.TextProperty, nameof(viewModel.TitleError)),

                new Label().Text("Description:").CenterVertical().FontSize(18),
                new Editor().CenterVertical().FontSize(14)
                    .Bind(Entry.TextProperty, nameof(viewModel.Description)),
                new Label().CenterVertical().FontSize(14).TextColor(Microsoft.Maui.Graphics.Colors.Red)
                    .Bind(Label.TextProperty, nameof(viewModel.DescriptionError)),

                new Label().Text("Date").CenterVertical().FontSize(18),
                datePicker.CenterVertical().FontSize(12)
                    .Bind(DatePicker.DateProperty, nameof(viewModel.Date)),

                new Label().Text("Time").CenterVertical().FontSize(18),
                new TimePicker().CenterVertical().FontSize(12)
                    .Bind(TimePicker.TimeProperty, nameof(viewModel.Time)),

                new Label().Text("Recurring").CenterVertical().FontSize(18),
                new Switch().Left().CenterVertical()
                    .Bind(Switch.IsToggledProperty, nameof(viewModel.IsRecurring)),

                buttonConfirm.Text("Confirm").CenterVertical()
                    .Bind(Button.CommandProperty, nameof(viewModel.ConfirmCommand))
            }        
        }.BackgroundColor(Colors.Snow).Margin(20);
    }
}