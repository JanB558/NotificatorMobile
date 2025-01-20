using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using NotificatorMobile.Converters;
using NotificatorMobile.Models;
using NotificatorMobile.Services;
using NotificatorMobile.Utilities;
using NotificatorMobile.ViewModels;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;

namespace NotificatorMobile.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;
        private readonly IServiceProvider _serviceProvider;

        public MainPage(Services.INotificationService notificationService, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _viewModel = new MainViewModel(notificationService);
            BindingContext = _viewModel;

            //content
            Content = 
                new Grid
                {
                    RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition(GridLength.Star)
                    },
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition(GridLength.Star)
                    },
                    Children =
                    {
                        new Label().Text(_viewModel.NoContent).FontSize(24)
                        .CenterVertical().CenterHorizontal()
                        .Bind(IsVisibleProperty, nameof(_viewModel.Notifications), BindingMode.OneWay, new NullToBooleanConverter()),
                        
                        new CollectionView
                        {
                            ItemTemplate = new DataTemplate(() =>
                            {
                                return new Border
                                {
                                    Stroke = Colors.LightGray,
                                    StrokeThickness = 1,
                                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                                    BackgroundColor = Colors.White,
                                    HorizontalOptions = LayoutOptions.Fill,
                                    Content = new Grid
                                    {
                                        Padding = 10,
                                        RowDefinitions = new RowDefinitionCollection
                                        {
                                            new RowDefinition(GridLength.Auto),
                                            new RowDefinition(GridLength.Auto),
                                            new RowDefinition(GridLength.Auto),
                                            new RowDefinition(GridLength.Auto)
                                        },
                                        ColumnDefinitions = new ColumnDefinitionCollection
                                        {
                                            new ColumnDefinition(GridLength.Star),
                                            new ColumnDefinition(GridLength.Auto)
                                        },
                                        Children =
                                        {
                                            new Label().Bind(Label.TextProperty, "Title")
                                            .Row(0).Column(0),
                                            new Label().Bind(Label.TextProperty, "Description")
                                            .Row(1).Column(0),
                                            new Label().Bind(Label.TextProperty, "TimeAndDate")
                                            .Row(2).Column(0),
                                            new Label().Bind(Label.IsVisibleProperty, "IsRecurring")
                                            .Row(3).Column(0).Text("Recurring"),
                                            new Button
                                            {
                                                TextColor = Colors.White,
                                                BackgroundColor = Colors.Red,
                                            }.Text("Delete")
                                            .Row(0).Column(1)
                                            .Bind(BindableObject.BindingContextProperty, ".")
                                            .Also(b => b.Clicked += async (sender, e) => await OnDeleteNotificationButtonClicked(sender ,e)),
                                            new Button
                                            {
                                                TextColor = Colors.White,
                                                BackgroundColor = Colors.Blue,
                                            }.Text("Update")
                                            .Row(3).Column(1)
                                            .Bind(BindableObject.BindingContextProperty, ".")
                                            .Also(b => b.Clicked += async (sender, e) => await OnUpdateNotificationButtonClicked(sender, e))
                                        }
                                    }
                                };
                            })
                        }
                        .Bind(ItemsView.ItemsSourceProperty, nameof(_viewModel.Notifications)),

                        new Button
                        {
                            CornerRadius = 50
                        }.Text("New notification").End().Bottom()
                        .Also(b => b.Clicked += async (sender, e) => await OnNewNotificationButtonClicked(sender, e))
                    }
                }.BackgroundColor(Colors.Snow).Margin(20);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.Initialize();
            if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }
            LocalNotificationCenter.Current.NotificationActionTapped += async (e) => await OnNotificationReceived(e);
        }

        private async Task OnNewNotificationButtonClicked(object? sender, EventArgs e)
        {
            var page = _serviceProvider.GetRequiredService<AddNotificationPage>();
            await Navigation.PushAsync(page);
        }

        private async Task OnUpdateNotificationButtonClicked(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.BindingContext is Notification notification)
            {
                var page = _serviceProvider.GetRequiredService<AddNotificationPage>();
                page.SetIsUpdate(notification);
                await Navigation.PushAsync(page);
            }
        }

        private async Task OnDeleteNotificationButtonClicked(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.BindingContext is Notification notification)
            {
                await _viewModel.Delete(notification.Id);
                await _viewModel.Initialize();
            }
        }

        private async Task OnNotificationReceived(NotificationEventArgs e)
        {
            int id = e.Request.NotificationId;
            var notification = await _viewModel.GetServiceHandle().GetById(id);
            if (notification == null) return;

            await _viewModel.Delete(id);
            if (notification.IsRecurring)
            {
                notification.TimeAndDate = notification.TimeAndDate.AddDays(1);
                await _viewModel.GetServiceHandle().Create(notification);
                await _viewModel.GetServiceHandle().Register(notification);
            }           
            await _viewModel.Initialize();
        }
    }
}
