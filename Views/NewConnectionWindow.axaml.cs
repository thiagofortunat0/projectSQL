using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SQLstudio.ViewModels;

namespace SQLstudio.Views
{
    public partial class NewConnectionWindow : Window
    {
        public NewConnectionWindow()
        {
            InitializeComponent();

            var viewModel = new NewConnectionViewModel();

            viewModel.DatabasesLoaded += databases => Close(databases);
            Closed += (_, _) => viewModel.ConnectCommand.Cancel();

            DataContext = viewModel;
        }

        public void CancelButton_Click(
            object? sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}