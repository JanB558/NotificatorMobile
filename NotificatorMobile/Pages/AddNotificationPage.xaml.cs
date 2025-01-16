using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Markup.LeftToRight;
using CommunityToolkit.Mvvm.Messaging;
using NotificatorMobile.Converters;
using NotificatorMobile.Services;
using NotificatorMobile.Utilities;
using NotificatorMobile.ViewModels;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace NotificatorMobile.Pages;

public partial class AddNotificationPage : ContentPage
{
    private readonly AddNotificationViewModel _viewModel;
    public AddNotificationPage(INotificationService notificationService)
	{
        _viewModel = new AddNotificationViewModel(notificationService);
        BindingContext = _viewModel;

        //declarations
        var buttonConfirm = new Button();
        buttonConfirm.CornerRadius = 50;
        var datePicker = new DatePicker();
        datePicker.MinimumDate = DateTime.Today;

        //content
        Content = new StackLayout
        {
            Children = 
            {
                new Label().Text("Title").CenterVertical().FontSize(18),
                new Entry().CenterVertical().FontSize(14)
                    .Bind(Entry.TextProperty, nameof(_viewModel.Title)),
                new Label().CenterVertical().FontSize(14).TextColor(Microsoft.Maui.Graphics.Colors.Red)
                    .Bind(Label.TextProperty, nameof(_viewModel.TitleError)),

                new Label().Text("Description").CenterVertical().FontSize(18),
                new Editor().CenterVertical().FontSize(14)
                    .Bind(Entry.TextProperty, nameof(_viewModel.Description)),
                new Label().CenterVertical().FontSize(14).TextColor(Microsoft.Maui.Graphics.Colors.Red)
                    .Bind(Label.TextProperty, nameof(_viewModel.DescriptionError)),

                new Label().Text("Date").CenterVertical().FontSize(18),
                datePicker.CenterVertical().FontSize(12)
                    .Bind(DatePicker.DateProperty, nameof(_viewModel.Date)),

                new Label().Text("Time").CenterVertical().FontSize(18),
                new TimePicker().CenterVertical().FontSize(12)
                    .Bind(TimePicker.TimeProperty, nameof(_viewModel.Time)),
                new Label().CenterVertical().FontSize(14).TextColor(Microsoft.Maui.Graphics.Colors.Red)
                    .Bind(Label.TextProperty, nameof(_viewModel.TimeError)),

                new Label().Text("Recurring").CenterVertical().FontSize(18),
                new Switch().Left().CenterVertical()
                    .Bind(Switch.IsToggledProperty, nameof(_viewModel.IsRecurring)),

                buttonConfirm.Text("Confirm").CenterVertical()
                    .Bind(Button.CommandProperty, nameof(_viewModel.ConfirmCommand))
            }        
        }.BackgroundColor(Colors.Snow).Margin(20);

        //define messages
        WeakReferenceMessenger.Default.Register<NavigateBackMessage>(this, (recipient, message) =>
        {
            if (message.Value)
            {
                MainThread.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
            }
        });
    }
}