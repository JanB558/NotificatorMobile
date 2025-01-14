using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using NotificatorMobile.ViewModels;

namespace NotificatorMobile.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel = new();

        public MainPage()
        {
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
                                new Label().Text(_viewModel.LabelText).FontSize(24).CenterHorizontal()
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
            await Navigation.PushAsync(new AddNotificationPage());
        }
    }
}
