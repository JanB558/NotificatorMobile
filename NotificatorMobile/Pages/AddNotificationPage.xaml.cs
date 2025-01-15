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
        var buttonConfirm = new Button();
        buttonConfirm.CornerRadius = 50;
        var datePicker = new DatePicker();
        datePicker.MinimumDate = DateTime.Today;

        var titleErrorLabel = new Label();
        titleErrorLabel.IsVisible = false;
        titleErrorLabel.TextColor = Microsoft.Maui.Graphics.Colors.Red;

        var descriptionErrorLabel = new Label();
        descriptionErrorLabel.IsVisible = false;
        descriptionErrorLabel.TextColor = Microsoft.Maui.Graphics.Colors.Red;

        //content
        Content = new StackLayout
        {
            Children = 
            {
                new Label().Text("Title:").CenterVertical().FontSize(18),
                new Entry().CenterVertical().FontSize(14)
                    .Bind(Entry.TextProperty, nameof(viewModel.Title)),
                titleErrorLabel.CenterVertical()
                    .Bind(Label.TextProperty, nameof(viewModel.TitleError))
                    .Bind(Label.IsVisibleProperty, nameof(viewModel.TitleError), converter: new NullToBooleanConverter()),

                new Label().Text("Description:").CenterVertical().FontSize(18),
                new Editor().CenterVertical().FontSize(14)
                    .Bind(Entry.TextProperty, nameof(viewModel.Description)),
                descriptionErrorLabel.CenterVertical()
                    .Bind(Label.TextProperty, nameof(viewModel.DescriptionError))
                    .Bind(Label.IsVisibleProperty, nameof(viewModel.DescriptionError), converter: new NullToBooleanConverter()),

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