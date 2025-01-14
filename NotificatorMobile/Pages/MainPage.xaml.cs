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
                    new Button().Text("+").Anchor(1, 1).LayoutBounds(0.95, 0.95, -1, -1).LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                }
            }.BackgroundColor(Colors.Snow);
        }
    }
}
