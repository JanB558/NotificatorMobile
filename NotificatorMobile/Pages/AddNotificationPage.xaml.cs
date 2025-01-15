using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Markup.LeftToRight;
using NotificatorMobile.ViewModels;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace NotificatorMobile.Pages;

public partial class AddNotificationPage : ContentPage
{
    private readonly AddNotificationViewModel viewModel = new();
    private enum Row 
    { 
        LabelTitle, InputTitle, LabelDescription, InputDescription, 
        LabelDate, InputDate, LabelTime, InputTime, LabelRecurring, InputRecurring,
        Button
    }
    private enum Column { Main }
    public AddNotificationPage()
	{
        BindingContext = viewModel;

        //declarations
        var buttonConfirm = new Button();
        buttonConfirm.CornerRadius = 50;
        var datePicker = new DatePicker();
        datePicker.MinimumDate = DateTime.Today;

        //content
        Content = new Grid
        {
            RowDefinitions = Rows.Define(
                (Row.LabelTitle, 36),
                (Row.InputTitle, Auto),
                (Row.LabelDescription, 36),
                (Row.InputDescription, Auto),
                (Row.LabelDate, 36),
                (Row.InputDate, Auto),
                (Row.LabelTime, 36),
                (Row.InputTime, Auto),
                (Row.LabelRecurring, 36),
                (Row.InputRecurring, Auto),
                (Row.Button, 50)
                ),
            ColumnDefinitions = Columns.Define(
                (Column.Main, Star)),
            Children = 
            {
                new Label().Text("Title:").CenterVertical().FontSize(18)
                    .Row(Row.LabelTitle).Column(Column.Main),
                new Entry().CenterVertical().FontSize(14)
                    .Bind(Entry.TextProperty, nameof(viewModel.Title))
                    .Row(Row.InputTitle).Column(Column.Main),

                new Label().Text("Description:").CenterVertical().FontSize(18)
                    .Row(Row.LabelDescription).Column(Column.Main),
                new Editor().CenterVertical().FontSize(14)
                    .Bind(Entry.TextProperty, nameof(viewModel.Description))
                    .Row(Row.InputDescription).Column(Column.Main),

                new Label().Text("Date").CenterVertical().FontSize(18)
                    .Row(Row.LabelDate).Column(Column.Main),
                datePicker.CenterVertical().FontSize(12)
                    .Bind(DatePicker.DateProperty, nameof(viewModel.Date))
                    .Row(Row.InputDate).Column(Column.Main),

                new Label().Text("Time").CenterVertical().FontSize(18)
                    .Row(Row.LabelTime).Column(Column.Main),
                new TimePicker().CenterVertical().FontSize(12)
                    .Bind(TimePicker.TimeProperty, nameof(viewModel.Time))
                    .Row(Row.InputTime).Column(Column.Main),

                new Label().Text("Recurring").CenterVertical().FontSize(18)
                    .Row(Row.LabelRecurring).Column(Column.Main),
                new Switch().Left().CenterVertical()
                    .Bind(Switch.IsToggledProperty, nameof(viewModel.IsRecurring))
                    .Row(Row.InputRecurring).Column(Column.Main),

                buttonConfirm.Text("Confirm").CenterVertical()
                    .Bind(Button.CommandProperty, nameof(viewModel.ConfirmCommand))
                    .Row(Row.Button).Column(Column.Main)
            }        
        }.BackgroundColor(Colors.Snow).Margin(20);
    }
}