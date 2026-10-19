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

            DataContext = new NewConnectionViewModel();
        }

        public void CancelButton_Click(
            object? sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}