using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Logging;
using NotificatorMobile.Pages;
using NotificatorMobile.Services;
using Plugin.LocalNotification;

namespace NotificatorMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitMarkup()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            //pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddNotificationPage>();
            //db
            builder.Services.AddSingleton<NotificatorMobile.Services.INotificationService>
                (provider => new NotificationService
                (Path.Combine(FileSystem.AppDataDirectory, "ndb.db"), "notifications"));

            return builder.Build();
        }
    }
}
