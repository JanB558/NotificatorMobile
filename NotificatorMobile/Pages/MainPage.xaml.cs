using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using NotificatorMobile.Converters;
using NotificatorMobile.Models;
using NotificatorMobile.Services;
using NotificatorMobile.Utilities;
using NotificatorMobile.ViewModels;

namespace NotificatorMobile.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;
        private readonly IServiceProvider _serviceProvider;

        public MainPage(INotificationService notificationService, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _viewModel = new MainViewModel(notificationService);
            BindingContext = _viewModel;

            //declarations

            //    var descriptionLabel = new Label
            //    {
            //        FontSize = 14,
            //        TextColor = Colors.Gray
            //    };
            //    descriptionLabel.SetBinding(Label.TextProperty, "Description");
            //    var timeAndDateLabel = new Label
            //    {
            //        FontSize = 12,
            //        TextColor = Colors.DarkGray
            //    };
            //    timeAndDateLabel.SetBinding(Label.TextProperty, new Binding("TimeAndDate", stringFormat: "{0:G}"));
            //    var recurringLabel = new Label
            //    {
            //        FontSize = 12,
            //        TextColor = Colors.DarkGray,
            //        IsVisible = false, 
            //        Text = "Recurring"
            //    };
            //    recurringLabel.SetBinding(Label.IsVisibleProperty, "IsRecurring");
            //    var buttonDelete = new Button
            //    {
            //        Text = "Delete",
            //        BackgroundColor = Colors.Red,
            //        TextColor = Colors.White,
            //        Padding = 5,
            //        FontSize = 12,
            //    };
            //    buttonDelete.Clicked += async (sender, e) =>
            //    {
            //        if (sender is Button btn && btn.BindingContext is Notification notification)
            //        {
            //            await _viewModel.Delete(notification.Id);
            //            await _viewModel.Initialize();
            //        }
            //    };
            //    buttonDelete.SetBinding(BindableObject.BindingContextProperty, ".");

            //    grid.Add(titleLabel, 0, 0);
            //    grid.Add(descriptionLabel, 0, 1);
            //    grid.Add(timeAndDateLabel, 0, 2);
            //    grid.Add(recurringLabel, 0, 3);
            //    grid.Add(buttonDelete, 1, 0);
            //    border.Content = grid;

            //    return border;
            //});

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
                                            .Row(0).Column(0)
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
        }

        private async void OnNewNotificationButtonClicked(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(_serviceProvider.GetRequiredService<AddNotificationPage>());
        }
    }
}
