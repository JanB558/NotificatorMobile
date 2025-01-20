using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Markup.LeftToRight;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Shapes;
using NotificatorMobile.Converters;
using NotificatorMobile.Models;
using NotificatorMobile.Services;
using NotificatorMobile.Utilities;
using NotificatorMobile.ViewModels;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace NotificatorMobile.Pages;

/// <summary>
/// serves for both add and update
/// </summary>
public partial class AddNotificationPage : ContentPage
{
    private readonly AddNotificationViewModel _viewModel;
    public AddNotificationPage(INotificationService notificationService)
	{
        _viewModel = new AddNotificationViewModel(notificationService);
        BindingContext = _viewModel;

        //content
        Content = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(new GridLength(15, GridUnitType.Star)),
                new RowDefinition(new GridLength(70, GridUnitType.Star)),
                new RowDefinition(new GridLength(15, GridUnitType.Star))
            },
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(GridLength.Star)
            },
            Children =
            {
                new Border
                {
                    Stroke = Colors.Snow,
                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                    StrokeThickness = 1,
                    Padding = 5,
                    BackgroundColor = Colors.Snow,
                    Content =
                    new StackLayout
                    {
                        Children =
                        {
                        new Label().FontSize(18).Bind(Label.TextProperty, nameof(_viewModel.Message))
                        .CenterVertical().CenterHorizontal(),

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
                        new DatePicker
                        {
                            MinimumDate = DateTime.Today,
                        }.CenterVertical().FontSize(12)
                        .Bind(DatePicker.DateProperty, nameof(_viewModel.Date)),

                        new Label().Text("Time").CenterVertical().FontSize(18),
                        new TimePicker().CenterVertical().FontSize(12)
                        .Bind(TimePicker.TimeProperty, nameof(_viewModel.Time)),
                        new Label().CenterVertical().FontSize(14).TextColor(Microsoft.Maui.Graphics.Colors.Red)
                        .Bind(Label.TextProperty, nameof(_viewModel.TimeError)),

                        new Label().Text("Recurring").CenterVertical().FontSize(18),
                        new Switch().Left().CenterVertical()
                        .Bind(Switch.IsToggledProperty, nameof(_viewModel.IsRecurring)),

                        new Button
                        {
                            CornerRadius = 50
                        }.Text("Confirm").CenterVertical()
                        .Bind(Button.CommandProperty, nameof(_viewModel.ConfirmCommand))
                        }
                    }
                }.Row(1).Column(0)
            }
        }.Margin(20);

        //define messages
        WeakReferenceMessenger.Default.Register<NavigateBackMessage>(this, (recipient, message) =>
        {
            if (message.Value)
            {
                MainThread.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
            }
        });
    }

    public void SetIsUpdate(Notification notification)
    {
        _viewModel.IsUpdate = true;
        _viewModel.IdForUpdate = notification.Id;
        _viewModel.Title = notification.Title;
        _viewModel.Description = notification.Description;
        _viewModel.Date = notification.TimeAndDate.Date;
        _viewModel.Time = notification.TimeAndDate.TimeOfDay;
        _viewModel.IsRecurring = notification.IsRecurring;

        _viewModel.Message = "Update notification";
    }
}