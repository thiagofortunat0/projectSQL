using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using SQLstudio.ViewModels;

namespace SQLstudio.Views.Controls
{
    public partial class NoDatabaseConnected : UserControl
    {
        public NoDatabaseConnected()
        {
            InitializeComponent();
        }

        private async void OpenNewConnection_Click(
            object? sender,
            RoutedEventArgs e)
        {
            if (TopLevel.GetTopLevel(this) is not Window owner) return;

            NewConnectionWindow modal = new();

            var databases = await modal.ShowDialog<List<string>?>(owner);
            if(databases is null)
            {
                return;
            }

            if(owner.DataContext is MainViewModel mainViewModel)
            {
                mainViewModel.LoadDatabases(databases);
            }
        }
    }
}