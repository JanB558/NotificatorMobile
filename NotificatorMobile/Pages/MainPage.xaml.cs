using CommunityToolkit.Maui.Markup;
using NotificatorMobile.ViewModels;

namespace NotificatorMobile.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel = new();

        public MainPage()
        {
            Content = new ScrollView
            {
                Content = new Label()
                    .Text(_viewModel.LabelText)
                    .Center()
                    .FontSize(24)
            }.BackgroundColor(Colors.Snow);
        }
    }

}
