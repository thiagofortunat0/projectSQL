using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SQLstudio.Views.Controls
{
    public partial class AppMenu : UserControl
    {
        public AppMenu()
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