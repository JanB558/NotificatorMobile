using CommunityToolkit.Maui.Markup;

namespace NotificatorMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitializeStyles();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        private void InitializeStyles()
        {
            Resources.Add("GlobalLabelStyle", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontSizeProperty, Value = 18 },
                    new Setter { Property = Label.FontFamilyProperty, Value = Microsoft.Maui.Font.Default }
                }
            });
        }
    }
}