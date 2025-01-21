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

        Color secondaryColor;
        Color primaryColor;
        if (Application.Current is not null)
        {
            secondaryColor = (Color)Application.Current.Resources["ColorSecondary"];
            primaryColor = (Color)Application.Current.Resources["ColorPrimary"];
        }
        else
        {
            secondaryColor = Colors.FloralWhite; //should never happen, but with this app will work anyway
            primaryColor = Colors.RoyalBlue;
        }

        //content
        Content = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(new GridLength(12, GridUnitType.Star)),
                new RowDefinition(new GridLength(76, GridUnitType.Star)),
                new RowDefinition(new GridLength(12, GridUnitType.Star))
            },
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(GridLength.Star)
            },
            Children =
            {
                new Border
                {
                    Stroke = secondaryColor,
                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                    StrokeThickness = 1,
                    Padding = 5,
                    BackgroundColor = secondaryColor,
                    Content =
                    new StackLayout
                    {
                        Children =
                        {
                        new Label().Bind(Label.TextProperty, nameof(_viewModel.Message))
                        .DynamicResource(VisualElement.StyleProperty, "HeaderLabelStyle"),

                        new Label().Text("Title").DynamicResource(VisualElement.StyleProperty, "StandardLabelStyle"),
                        new Entry().Bind(Entry.TextProperty, nameof(_viewModel.Title))
                        .DynamicResource(VisualElement.StyleProperty, "EntryStyle"),
                        new Label().DynamicResource(VisualElement.StyleProperty, "ErrorLabelStyle")
                        .Bind(Label.TextProperty, nameof(_viewModel.TitleError)),

                        new Label().Text("Description").DynamicResource(VisualElement.StyleProperty, "StandardLabelStyle"),
                        new Editor().DynamicResource(VisualElement.StyleProperty, "EditorStyle")
                        .Bind(Entry.TextProperty, nameof(_viewModel.Description)),
                        new Label().DynamicResource(VisualElement.StyleProperty, "ErrorLabelStyle")
                        .Bind(Label.TextProperty, nameof(_viewModel.DescriptionError)),

                        new Label().Text("Date").DynamicResource(VisualElement.StyleProperty, "StandardLabelStyle"),
                        new DatePicker
                        {
                            MinimumDate = DateTime.Today,
                        }
                        .DynamicResource(VisualElement.StyleProperty, "DatePickerStyle")
                        .Bind(DatePicker.DateProperty, nameof(_viewModel.Date)),

                        new Label().Text("Time").DynamicResource(VisualElement.StyleProperty, "StandardLabelStyle"),
                        new TimePicker().DynamicResource(VisualElement.StyleProperty, "TimePickerStyle")
                        .Bind(TimePicker.TimeProperty, nameof(_viewModel.Time)),
                        new Label().DynamicResource(VisualElement.StyleProperty, "ErrorLabelStyle")
                        .Bind(Label.TextProperty, nameof(_viewModel.TimeError)),

                        new Label().Text("Recurring").DynamicResource(VisualElement.StyleProperty, "StandardLabelStyle"),
                        new Switch().Left().CenterVertical()
                        .Bind(Switch.IsToggledProperty, nameof(_viewModel.IsRecurring)),

                        new Button
                        {
                            CornerRadius = 50,
                            BackgroundColor = primaryColor
                        }.Text("Confirm").Bottom().CenterHorizontal()
                        .DynamicResource(VisualElement.StyleProperty, "ButtonStyle")
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