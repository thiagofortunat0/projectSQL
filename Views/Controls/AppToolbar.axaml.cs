using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SQLstudio.Views;

namespace SQLstudio.Views.Controls
{
    public partial class AppToolbar : UserControl
    {
        public AppToolbar()
        {
            InitializeComponent();
        }

        private async void OpenNewConnection_Click(
            object? sender,
            RoutedEventArgs e)
        {
            if (TopLevel.GetTopLevel(this) is not Window owner) return;

            NewConnectionWindow modal = new();

            await modal.ShowDialog(owner);
        }
    }
}