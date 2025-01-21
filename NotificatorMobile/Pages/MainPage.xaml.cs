using CommunityToolkit.Maui.Markup;
using MauiIcons.Core;
using MauiIcons.Material;
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

            var screenWidth = DeviceDisplay.MainDisplayInfo.Width;
            var screenHeight = DeviceDisplay.MainDisplayInfo.Height;
            var buttonSize = Math.Min(screenWidth, screenHeight) * 0.055;

            Color secondaryColor;
            if (Application.Current is not null)
                secondaryColor = (Color)Application.Current.Resources["ColorSecondary"];
            else secondaryColor = Colors.White; //should never happen, but with this app will work anyway

            //content
            Content = new Grid
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
                    new Label().Text(_viewModel.NoContent)
                    .DynamicResource(VisualElement.StyleProperty, "HeaderLabelStyle")
                    .Bind(IsVisibleProperty, nameof(_viewModel.Notifications), BindingMode.OneWay, new NullToBooleanConverter()),
                    
                    new CollectionView
                    {
                        ItemTemplate = new DataTemplate(() =>
                        {
                            return new Border
                            {
                                Stroke = secondaryColor,
                                StrokeShape = new RoundRectangle { CornerRadius = 10 },
                                StrokeThickness = 1,
                                Padding = 5,
                                BackgroundColor = Colors.FloralWhite,
                                Content = new Grid
                                {
                                    Padding = 10,
                                    RowDefinitions = new RowDefinitionCollection
                                    {
                                        new RowDefinition(new GridLength(20, GridUnitType.Star)),
                                        new RowDefinition(GridLength.Auto),
                                        new RowDefinition(new GridLength(20, GridUnitType.Star)),
                                        new RowDefinition(new GridLength(20, GridUnitType.Star))
                                    },
                                    ColumnDefinitions = new ColumnDefinitionCollection
                                    {
                                        new ColumnDefinition(new GridLength(90, GridUnitType.Star)),
                                        new ColumnDefinition(new GridLength(10, GridUnitType.Star))
                                    },
                                    Children =
                                    {
                                        new Label().Bind(Label.TextProperty, "Title")
                                        .DynamicResource(VisualElement.StyleProperty, "HeaderLabelStyle")
                                        .Row(0).Column(0),
                                        new Label().Bind(Label.TextProperty, "Description")
                                        .DynamicResource(VisualElement.StyleProperty, "StandardLabelStyle")
                                        .Row(1).Column(0),
                                        new Label().Bind(Label.TextProperty, "TimeAndDate")
                                        .DynamicResource(VisualElement.StyleProperty, "StandardLabelStyle")
                                        .Row(2).Column(0),
                                        new Label().Bind(Label.IsVisibleProperty, "IsRecurring")
                                        .DynamicResource(VisualElement.StyleProperty, "StandardLabelStyle")
                                        .Row(3).Column(0).Text("Recurring"),
                                        new Button
                                        {
                                            ImageSource =
                                            (Microsoft.Maui.Controls.ImageSource)
                                            new MauiIcon { Icon = MaterialIcons.Delete, IconColor = Colors.Crimson },
                                            BackgroundColor = Colors.Transparent,
                                            BorderWidth = 0,
                                            Padding = 0
                                        }.Row(0).Column(1)
                                        .Bind(BindableObject.BindingContextProperty, ".")
                                        .Also(b => b.Clicked += async (sender, e) => await OnDeleteNotificationButtonClicked(sender ,e)),
                                        new Button
                                        {
                                            ImageSource =
                                            (Microsoft.Maui.Controls.ImageSource)
                                            new MauiIcon { Icon = MaterialIcons.Edit, IconColor = Colors.RoyalBlue },
                                            BackgroundColor = Colors.Transparent,
                                            BorderWidth = 0,
                                            Padding = 0
                                        }.Row(3).Column(1)
                                        .Bind(BindableObject.BindingContextProperty, ".")
                                        .Also(b => b.Clicked += async (sender, e) => await OnUpdateNotificationButtonClicked(sender, e))
                                    }
                                }
                            };
                        })
                    }.Bind(ItemsView.ItemsSourceProperty, nameof(_viewModel.Notifications)),

                    new Button
                    {
                        WidthRequest = buttonSize,
                        HeightRequest = buttonSize,
                        CornerRadius = (int)(buttonSize / 2.0),
                        ImageSource = 
                        (Microsoft.Maui.Controls.ImageSource)
                        new MauiIcon { Icon = MaterialIcons.Add, IconColor = Colors.White }
                    }.End().Bottom()
                    .DynamicResource(VisualElement.StyleProperty, "ButtonStyle")
                    .Also(b => b.Clicked += async (sender, e) => await OnNewNotificationButtonClicked(sender, e))
                }
            }.Margin(20);
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
