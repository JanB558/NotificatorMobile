using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
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
                                new Label().Text(_viewModel.NoContent).FontSize(24).Center()
                            }
                        }
                    },
                    buttonCorner.Text("New notification")
                    .LayoutBounds(1, 1, -1, -1).LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                }
            }.BackgroundColor(Colors.Snow).Margin(20);
        }

        private async void OnNewNotificationButtonClicked(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(_serviceProvider.GetRequiredService<AddNotificationPage>());
        }
    }
}
