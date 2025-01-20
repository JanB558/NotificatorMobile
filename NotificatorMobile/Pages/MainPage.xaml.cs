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
                                            .Also(b => b.Clicked += OnDeleteNotificationButtonClicked),
                                            new Button
                                            {
                                                TextColor = Colors.White,
                                                BackgroundColor = Colors.Blue,
                                            }.Text("Update")
                                            .Row(3).Column(1)
                                            .Bind(BindableObject.BindingContextProperty, ".")
                                            .Also(b => b.Clicked += OnUpdateNotificationButtonClicked)
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
                        .Also(b => b.Clicked += OnNewNotificationButtonClicked)
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
        }

        private async void OnNewNotificationButtonClicked(object? sender, EventArgs e)
        {
            var page = _serviceProvider.GetRequiredService<AddNotificationPage>();
            await Navigation.PushAsync(page);
        }

        private async void OnUpdateNotificationButtonClicked(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.BindingContext is Notification notification)
            {
                var page = _serviceProvider.GetRequiredService<AddNotificationPage>();
                page.SetIsUpdate(notification);
                await Navigation.PushAsync(page);
            }
        }

        private async void OnDeleteNotificationButtonClicked(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.BindingContext is Notification notification)
            {
                await _viewModel.Delete(notification.Id);
                await _viewModel.Initialize();
            }
        }
    }
}
