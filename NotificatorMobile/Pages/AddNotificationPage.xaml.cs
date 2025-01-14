using CommunityToolkit.Maui.Markup;
using NotificatorMobile.ViewModels;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace NotificatorMobile.Pages;

public partial class AddNotificationPage : ContentPage
{
    private readonly AddNotificationViewModel _viewModel = new();
    private enum Row { LabelTitle, InputTitle }
    private enum Column { Main }
    public AddNotificationPage()
	{
        Content = new Grid
        {
            RowDefinitions = Rows.Define(
                (Row.LabelTitle, 36)),
            ColumnDefinitions = Columns.Define(
                (Column.Main, Star)),
            Children = 
            {
                new Label()
                    .Text("Title:")
                    .Center()
                    .FontSize(12)
                    .Row(Row.LabelTitle).Column(Column.Main)
            }        
        }.BackgroundColor(Colors.Snow);
    }
}