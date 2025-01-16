using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using NotificatorMobile.Converters;
using NotificatorMobile.Services;
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
            var buttonCorner = new Button();
            buttonCorner.Clicked += OnNewNotificationButtonClicked;
            buttonCorner.CornerRadius = 50;

            var collectionView = new CollectionView();
            collectionView.ItemTemplate = new DataTemplate(() =>
            {
                var border = new Border
                {
                    Stroke = Colors.LightGray,
                    StrokeThickness = 1,
                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                    BackgroundColor = Colors.White
                };
                var stackLayout = new VerticalStackLayout
                {
                    Padding = 10,
                    Spacing = 5
                };

                var titleLabel = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 18,
                    TextColor = Colors.Black
                };
                titleLabel.SetBinding(Label.TextProperty, "Title");
                var descriptionLabel = new Label
                {
                    FontSize = 14,
                    TextColor = Colors.Gray
                };
                descriptionLabel.SetBinding(Label.TextProperty, "Description");
                var timeAndDateLabel = new Label
                {
                    FontSize = 12,
                    TextColor = Colors.DarkGray
                };
                timeAndDateLabel.SetBinding(Label.TextProperty, new Binding("TimeAndDate", stringFormat: "{0:G}"));
                var recurringLabel = new Label
                {
                    FontSize = 12,
                    TextColor = Colors.DarkGray,
                    IsVisible = false, 
                    Text = "Recurring"
                };
                recurringLabel.SetBinding(Label.IsVisibleProperty, "IsRecurring");

                stackLayout.Add(titleLabel);
                stackLayout.Add(descriptionLabel);
                stackLayout.Add(timeAndDateLabel);
                stackLayout.Add(recurringLabel);
                border.Content = stackLayout;

                return border;
            });

            //content
            Content = new AbsoluteLayout
            {
                Children =
                {
                    new ScrollView
                    {
                        Content = new StackLayout
                        {
                            Children =
                            {
                                new Label().Text(_viewModel.NoContent).FontSize(24)
                                .CenterVertical().CenterHorizontal()
                                .Bind(IsVisibleProperty, nameof(_viewModel.Notifications), BindingMode.OneWay, new NullToBooleanConverter()),

                                collectionView
                                    .Bind(ItemsView.ItemsSourceProperty, nameof(_viewModel.Notifications))
                            }
                        }
                    },
                    buttonCorner.Text("New notification")
                    .LayoutBounds(1, 1, -1, -1).LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
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
